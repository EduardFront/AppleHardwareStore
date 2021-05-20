using Microsoft.EntityFrameworkCore.Migrations;

namespace AppleHardwareStore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "order_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_status_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_type_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    client_phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    client_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    client_card_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_cost = table.Column<double>(type: "float", nullable: false),
                    order_status_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_order_status_order_status_id",
                        column: x => x.order_status_id,
                        principalTable: "order_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: true),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_product_type_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "product_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "position",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    count = table.Column<int>(type: "int", nullable: false),
                    cost = table.Column<double>(type: "float", nullable: false),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("position_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_position_order_order_id",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_position_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_order_status_id",
                table: "order",
                column: "order_status_id");

            migrationBuilder.CreateIndex(
                name: "idx_order_id",
                table: "position",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_id",
                table: "position",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_type_id",
                table: "product",
                column: "product_type_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "position");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "order_status");

            migrationBuilder.DropTable(
                name: "product_type");
        }
    }
}
