using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetRFID.Data.Migrations
{
    public partial class KNN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analytique",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    precison = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analytique", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "KNN",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    n_neighbors = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    metric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    p = table.Column<float>(type: "real", nullable: false),
                    metric_params = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    algorithm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    leaf_size = table.Column<int>(type: "int", nullable: false),
                    precision = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KNN", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Random_Forest",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    n_estimators = table.Column<int>(type: "int", nullable: false),
                    criterion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    min_samples_split = table.Column<int>(type: "int", nullable: false),
                    min_samples_leaf = table.Column<int>(type: "int", nullable: false),
                    min_weight_fraction_leaf = table.Column<int>(type: "int", nullable: false),
                    max_leaf_nodes = table.Column<int>(type: "int", nullable: false),
                    min_impurity_decrease = table.Column<float>(type: "real", nullable: false),
                    n_jobs = table.Column<int>(type: "int", nullable: false),
                    entier_detail = table.Column<int>(type: "int", nullable: false),
                    max_depth = table.Column<int>(type: "int", nullable: false),
                    precision = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Random_Forest", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SVM",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C = table.Column<float>(type: "real", nullable: false),
                    kernel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gamma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    coef0 = table.Column<float>(type: "real", nullable: false),
                    tol = table.Column<float>(type: "real", nullable: false),
                    cache_size = table.Column<float>(type: "real", nullable: false),
                    max_iter = table.Column<int>(type: "int", nullable: false),
                    decision_function_shape = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precision = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SVM", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Simulation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idA = table.Column<int>(type: "int", nullable: false),
                    idS = table.Column<int>(type: "int", nullable: false),
                    idk = table.Column<int>(type: "int", nullable: false),
                    idR = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulation", x => x.id);
                    table.ForeignKey(
                        name: "FK_Simulation_Analytique_idA",
                        column: x => x.idA,
                        principalTable: "Analytique",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Simulation_KNN_idk",
                        column: x => x.idk,
                        principalTable: "KNN",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Simulation_Random_Forest_idR",
                        column: x => x.idR,
                        principalTable: "Random_Forest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Simulation_SVM_idS",
                        column: x => x.idS,
                        principalTable: "SVM",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Simulation_idA",
                table: "Simulation",
                column: "idA");

            migrationBuilder.CreateIndex(
                name: "IX_Simulation_idk",
                table: "Simulation",
                column: "idk");

            migrationBuilder.CreateIndex(
                name: "IX_Simulation_idR",
                table: "Simulation",
                column: "idR");

            migrationBuilder.CreateIndex(
                name: "IX_Simulation_idS",
                table: "Simulation",
                column: "idS");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Simulation");

            migrationBuilder.DropTable(
                name: "Analytique");

            migrationBuilder.DropTable(
                name: "KNN");

            migrationBuilder.DropTable(
                name: "Random_Forest");

            migrationBuilder.DropTable(
                name: "SVM");
        }
    }
}
