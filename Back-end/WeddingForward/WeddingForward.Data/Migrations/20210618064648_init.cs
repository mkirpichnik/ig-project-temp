using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingForward.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountsSession",
                columns: table => new
                {
                    SessionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAlive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsSession", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLogsHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLogsHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostCheckDetailsSet",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCheckDetailsSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScriptsSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScriptType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Args = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanedStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsStarted = table.Column<bool>(type: "bit", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptsSchedule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Follow = table.Column<int>(type: "int", nullable: false),
                    Following = table.Column<int>(type: "int", nullable: false),
                    PostsCount = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    ProfilePicUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicUrlHD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "AccountLinkedPosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerUsername = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLinkedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountLinkedPosts_UserAccounts_OwnerUsername",
                        column: x => x.OwnerUsername,
                        principalTable: "UserAccounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerUsername = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_UserAccounts_OwnerUsername",
                        column: x => x.OwnerUsername,
                        principalTable: "UserAccounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostCheckerHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostCheckDetailsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CheckedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCheckerHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostCheckerHistory_PostCheckDetailsSet_PostCheckDetailsId",
                        column: x => x.PostCheckDetailsId,
                        principalTable: "PostCheckDetailsSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostCheckerHistory_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostCheckerHistory_UserAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostContent",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostContent_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tagged",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostContentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tagged", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tagged_PostContent_PostContentId",
                        column: x => x.PostContentId,
                        principalTable: "PostContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountLinkedPosts_OwnerUsername",
                table: "AccountLinkedPosts",
                column: "OwnerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_PostCheckerHistory_AccountId",
                table: "PostCheckerHistory",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCheckerHistory_PostCheckDetailsId",
                table: "PostCheckerHistory",
                column: "PostCheckDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PostCheckerHistory_PostId",
                table: "PostCheckerHistory",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostContent_PostId",
                table: "PostContent",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_OwnerUsername",
                table: "Posts",
                column: "OwnerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Tagged_PostContentId",
                table: "Tagged",
                column: "PostContentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLinkedPosts");

            migrationBuilder.DropTable(
                name: "AccountsSession");

            migrationBuilder.DropTable(
                name: "ApplicationLogsHistory");

            migrationBuilder.DropTable(
                name: "PostCheckerHistory");

            migrationBuilder.DropTable(
                name: "ScriptsSchedule");

            migrationBuilder.DropTable(
                name: "Tagged");

            migrationBuilder.DropTable(
                name: "PostCheckDetailsSet");

            migrationBuilder.DropTable(
                name: "PostContent");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
