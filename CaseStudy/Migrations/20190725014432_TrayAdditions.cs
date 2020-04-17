using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseStudy.Migrations
{
    public partial class TrayAdditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           

            migrationBuilder.CreateTable(
                name: "Trays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderAmount = table.Column<decimal>(type: "money", nullable: false),
                    UserId = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trays", x => x.Id);
                });


            migrationBuilder.CreateTable(
                name: "TrayItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(nullable: false),
                    QtyOrdered = table.Column<int>(maxLength: 15, nullable: false),
                    QtySold = table.Column<int>(nullable: false),
                    QtyBackOrdered = table.Column<int>(nullable: false),
                    SellingPrice = table.Column<decimal>(type: "money", nullable: false),
                    ProductId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrayItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrayItems_Trays_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Trays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrayItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });



            migrationBuilder.CreateIndex(
                name: "IX_TrayItems_OrderId",
                table: "TrayItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrayItems_ProductId",
                table: "TrayItems",
                column: "ProductId");
        }
    }
}
