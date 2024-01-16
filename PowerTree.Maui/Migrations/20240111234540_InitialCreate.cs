using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PowerTree.Maui.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pthierarchy",
                columns: table => new
                {
                    HierarchyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10, 1"),
                    HierarchyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subsystem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pthierarchy", x => x.HierarchyId);
                });

            migrationBuilder.CreateTable(
                name: "ptnode",
                columns: table => new
                {
                    NodeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10000, 1"),
                    HierarchyId = table.Column<int>(type: "int", nullable: false),
                    NodeOrder = table.Column<int>(type: "int", nullable: false),
                    NodeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentNodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ptnode", x => x.NodeId);
                    table.ForeignKey(
                        name: "FK_ptnode_pthierarchy_HierarchyId",
                        column: x => x.HierarchyId,
                        principalTable: "pthierarchy",
                        principalColumn: "HierarchyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ptnode_ptnode_ParentNodeId",
                        column: x => x.ParentNodeId,
                        principalTable: "ptnode",
                        principalColumn: "NodeId");
                });

            migrationBuilder.CreateTable(
                name: "ptnodeitem",
                columns: table => new
                {
                    NodeItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NodeItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    NodeId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    NodeImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ptnodeitem", x => x.NodeItemId);
                    table.ForeignKey(
                        name: "FK_ptnodeitem_ptnode_NodeId",
                        column: x => x.NodeId,
                        principalTable: "ptnode",
                        principalColumn: "NodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pthierarchy_Subsystem_HierarchyName",
                table: "pthierarchy",
                columns: new[] { "Subsystem", "HierarchyName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ptnode_HierarchyId",
                table: "ptnode",
                column: "HierarchyId");

            migrationBuilder.CreateIndex(
                name: "IX_ptnode_ParentNodeId",
                table: "ptnode",
                column: "ParentNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ptnodeitem_NodeId",
                table: "ptnodeitem",
                column: "NodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ptnodeitem");

            migrationBuilder.DropTable(
                name: "ptnode");

            migrationBuilder.DropTable(
                name: "pthierarchy");
        }
    }
}
