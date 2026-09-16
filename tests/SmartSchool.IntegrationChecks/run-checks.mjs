// Runs real API handlers, EF writes and Dapper reads on an isolated embedded PostgreSQL database.
// Test authentication and account provisioning are intentionally confined to this test project.
import { PGlite } from '@electric-sql/pglite';
import { pgcrypto } from '@electric-sql/pglite/contrib/pgcrypto';
import { vector } from '@electric-sql/pglite-pgvector';
import { PGLiteSocketServer } from '@electric-sql/pglite-socket';
import { spawn } from 'node:child_process';
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
const here = path.dirname(fileURLToPath(import.meta.url));
const root = path.resolve(here, '../..');
const dotnet = process.env.DOTNET_EXECUTABLE || 'dotnet';
async function run(args, env = {}) {
  const child = spawn(dotnet, args, { cwd: root, stdio: 'inherit', env: { ...process.env, ...env } });
  return await new Promise((resolve, reject) => { child.once('error', reject); child.once('exit', code => resolve(code ?? 1)); });
}
if (!process.argv.includes('--no-build')) {
  const code = await run(['build', here, '--nologo', '-m:1', '-nr:false', '-v', 'minimal']);
  if (code) process.exit(code);
}
const db = await PGlite.create({ extensions: { pgcrypto, vector } });
let server;
let code = 1;
try {
  await db.exec(fs.readFileSync(path.join(root, 'database/SmartSchool.FreshInstall.sql'), 'utf8'));
  console.log('PASS: Fresh database installation');
  await db.exec(fs.readFileSync(path.join(root, 'database/postgresql/V123__persisted_school_workflows.sql'), 'utf8'));
  console.log('PASS: V123 migration is repeatable');
  console.log('Database:', (await db.query('SELECT version() AS version')).rows[0].version);
  server = new PGLiteSocketServer({ db, host: '127.0.0.1', port: 5433, maxConnections: 20 });
  await server.start();
  const reportDirectory = path.join(root, 'test-results'); fs.mkdirSync(reportDirectory, { recursive: true });
  code = await run([path.join(here, 'bin/Debug/net10.0/SmartSchool.IntegrationChecks.dll')], {
    SMARTSCHOOL_TEST_DATABASE: 'Host=127.0.0.1;Port=5433;Database=postgres;Username=postgres;Password=postgres;SSL Mode=Disable;Pooling=false;Include Error Detail=true',
    SMARTSCHOOL_TEST_REPORT: path.join(reportDirectory, 'workflow-checks.json'),
  });
} finally {
  if (server) await server.stop();
  await db.close();
}
process.exit(code);
