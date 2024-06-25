using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChoreographySaga.OrdersService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orders");

            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "orders",
                columns: table => new
                {
                    OrderUuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustomerUuid = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderUuid);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                schema: "orders",
                columns: table => new
                {
                    OrderProductUuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    ProductUuid = table.Column<Guid>(type: "char(36)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderUuid = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => x.OrderProductUuid);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Order_OrderUuid",
                        column: x => x.OrderUuid,
                        principalSchema: "orders",
                        principalTable: "Order",
                        principalColumn: "OrderUuid");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderUuid",
                schema: "orders",
                table: "Order",
                column: "OrderUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_OrderProductUuid",
                schema: "orders",
                table: "OrderProduct",
                column: "OrderProductUuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_OrderUuid",
                schema: "orders",
                table: "OrderProduct",
                column: "OrderUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProduct",
                schema: "orders");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "orders");
        }
    }
}
