using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeThanhPhuongMVC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CategoryDescription = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    ParentCategoryID = table.Column<short>(type: "INTEGER", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryID",
                        column: x => x.ParentCategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SystemAccount",
                columns: table => new
                {
                    AccountID = table.Column<short>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountName = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    AccountEmail = table.Column<string>(type: "TEXT", maxLength: 90, nullable: false),
                    AccountRole = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountPassword = table.Column<string>(type: "TEXT", maxLength: 90, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAccount", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TagName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "NewsArticles",
                columns: table => new
                {
                    NewsArticleID = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NewsTitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Headline = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    NewsContent = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    NewsSource = table.Column<string>(type: "TEXT", maxLength: 800, nullable: true),
                    CategoryID = table.Column<short>(type: "INTEGER", nullable: false),
                    NewsStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedByID = table.Column<short>(type: "INTEGER", nullable: false),
                    UpdatedByID = table.Column<short>(type: "INTEGER", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.NewsArticleID);
                    table.ForeignKey(
                        name: "FK_NewsArticles_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsArticles_SystemAccount_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "SystemAccount",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsArticles_SystemAccount_UpdatedByID",
                        column: x => x.UpdatedByID,
                        principalTable: "SystemAccount",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsTags",
                columns: table => new
                {
                    NewsArticleID = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TagID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsTags", x => new { x.NewsArticleID, x.TagID });
                    table.ForeignKey(
                        name: "FK_NewsTags_NewsArticles_NewsArticleID",
                        column: x => x.NewsArticleID,
                        principalTable: "NewsArticles",
                        principalColumn: "NewsArticleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryID", "CategoryDescription", "CategoryName", "IsActive", "ParentCategoryID" },
                values: new object[,]
                {
                    { (short)1, "General news and announcements", "General News", true, null },
                    { (short)2, "Academic related news and updates", "Academic News", true, null },
                    { (short)3, "Student activities and events", "Student Activities", true, null }
                });

            migrationBuilder.InsertData(
                table: "SystemAccount",
                columns: new[] { "AccountID", "AccountEmail", "AccountName", "AccountPassword", "AccountRole" },
                values: new object[] { (short)1, "admin@FUNewsManagementSystem.org", "System Administrator", "@@abc123@@", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryID",
                table: "Categories",
                column: "ParentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_CategoryID",
                table: "NewsArticles",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_CreatedByID",
                table: "NewsArticles",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_UpdatedByID",
                table: "NewsArticles",
                column: "UpdatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTags_TagID",
                table: "NewsTags",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_SystemAccount_AccountEmail",
                table: "SystemAccount",
                column: "AccountEmail",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsTags");

            migrationBuilder.DropTable(
                name: "NewsArticles");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "SystemAccount");
        }
    }
}
