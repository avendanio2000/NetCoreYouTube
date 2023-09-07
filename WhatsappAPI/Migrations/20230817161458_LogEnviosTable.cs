using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappAPI.Migrations
{
    /// <inheritdoc />
    public partial class LogEnviosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogEnvios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioAPI = table.Column<int>(type: "int", nullable: false),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NroCelular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreDestinatario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoRespueta = table.Column<int>(type: "int", nullable: false),
                    DescripcionRespuesta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEnvios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogEnvios");
        }
    }
}
