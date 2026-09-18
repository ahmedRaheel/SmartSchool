using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SmartSchool.Modules.Inventory.Persistence;

#nullable disable

namespace SmartSchool.Modules.Inventory.Persistence.Migrations.PostgreSql;

[DbContext(typeof(InventoryDbContext))]
[Migration("20260918154100_AlignInventoryNaming")]
public sealed class AlignInventoryNaming : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF to_regclass('inventory."StockTransaction"') IS NOT NULL
                   AND to_regclass('inventory.stock_transaction') IS NULL THEN
                    ALTER TABLE inventory."StockTransaction"
                    RENAME TO stock_transaction;
                END IF;
            END $$;
            """);

        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'purchase_order'
                      AND column_name = 'MetadataJson')
                   AND NOT EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'purchase_order'
                      AND column_name = 'metadata_json') THEN
                    ALTER TABLE inventory.purchase_order
                    RENAME COLUMN "MetadataJson" TO metadata_json;
                END IF;
            END $$;
            """);

        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'stock_transaction'
                      AND column_name = 'MetadataJson')
                   AND NOT EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'stock_transaction'
                      AND column_name = 'metadata_json') THEN
                    ALTER TABLE inventory.stock_transaction
                    RENAME COLUMN "MetadataJson" TO metadata_json;
                END IF;
            END $$;
            """);

        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'purchase_order'
                      AND column_name = 'metadata_json'
                      AND data_type <> 'jsonb') THEN
                    ALTER TABLE inventory.purchase_order
                    ALTER COLUMN metadata_json TYPE jsonb
                    USING CASE
                        WHEN metadata_json IS NULL OR btrim(metadata_json::text) = '' THEN NULL
                        ELSE metadata_json::text::jsonb
                    END;
                END IF;
            END $$;
            """);

        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'stock_transaction'
                      AND column_name = 'metadata_json'
                      AND data_type <> 'jsonb') THEN
                    ALTER TABLE inventory.stock_transaction
                    ALTER COLUMN metadata_json TYPE jsonb
                    USING CASE
                        WHEN metadata_json IS NULL OR btrim(metadata_json::text) = '' THEN NULL
                        ELSE metadata_json::text::jsonb
                    END;
                END IF;
            END $$;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'purchase_order'
                      AND column_name = 'metadata_json')
                   AND NOT EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'purchase_order'
                      AND column_name = 'MetadataJson') THEN
                    ALTER TABLE inventory.purchase_order
                    ALTER COLUMN metadata_json TYPE text
                    USING metadata_json::text;

                    ALTER TABLE inventory.purchase_order
                    RENAME COLUMN metadata_json TO "MetadataJson";
                END IF;
            END $$;
            """);

        migrationBuilder.Sql("""
            DO $$
            BEGIN
                IF EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'stock_transaction'
                      AND column_name = 'metadata_json')
                   AND NOT EXISTS (
                    SELECT 1
                    FROM information_schema.columns
                    WHERE table_schema = 'inventory'
                      AND table_name = 'stock_transaction'
                      AND column_name = 'MetadataJson') THEN
                    ALTER TABLE inventory.stock_transaction
                    ALTER COLUMN metadata_json TYPE text
                    USING metadata_json::text;

                    ALTER TABLE inventory.stock_transaction
                    RENAME COLUMN metadata_json TO "MetadataJson";
                END IF;

                IF to_regclass('inventory.stock_transaction') IS NOT NULL
                   AND to_regclass('inventory."StockTransaction"') IS NULL THEN
                    ALTER TABLE inventory.stock_transaction
                    RENAME TO "StockTransaction";
                END IF;
            END $$;
            """);
    }
}
