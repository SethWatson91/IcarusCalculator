using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Icarus_Item_Calculator.Migrations
{
    /// <inheritdoc />
    public partial class AddedIdentityFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBaseItem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.RecipeId);
                    table.ForeignKey(
                        name: "FK_Recipes_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeItems",
                columns: table => new
                {
                    RecipeItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeItems", x => x.RecipeItemId);
                    table.ForeignKey(
                        name: "FK_RecipeItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeItems_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "IsBaseItem", "Name" },
                values: new object[,]
                {
                    { 1, false, "Aluminium Ingot" },
                    { 2, true, "Aluminum Ore" },
                    { 3, false, "Basic Fertilizer" },
                    { 4, true, "Bone" },
                    { 5, false, "Carbon Fiber" },
                    { 6, false, "Carbon Paste" },
                    { 7, true, "Charcoal" },
                    { 8, true, "Coal Ore" },
                    { 9, true, "Cocoa Seed" },
                    { 10, true, "Coffee Bean" },
                    { 11, false, "Composite Paste" },
                    { 12, false, "Composites" },
                    { 13, false, "Concrete Mix" },
                    { 14, false, "Copper Ingot" },
                    { 15, false, "Copper Nail" },
                    { 16, true, "Copper Ore" },
                    { 17, false, "Copper Wire" },
                    { 18, true, "Corn" },
                    { 19, false, "Crushed Bone" },
                    { 20, true, "Dirt" },
                    { 21, false, "Electronics" },
                    { 22, false, "Epoxy" },
                    { 23, true, "Exotics" },
                    { 24, true, "Fiber" },
                    { 25, true, "Fish Chunks" },
                    { 26, false, "Flour" },
                    { 27, true, "Fur" },
                    { 28, false, "Glass" },
                    { 29, true, "Gold Ore" },
                    { 30, false, "Gold Wire" },
                    { 31, true, "Gorse Flower" },
                    { 32, false, "Growth Fertilizer" },
                    { 33, false, "Gunpowder" },
                    { 34, false, "High-Quality Fertilizer" },
                    { 35, true, "Ice" },
                    { 36, false, "Iron Ingot" },
                    { 37, false, "Iron Nail" },
                    { 38, true, "Iron Ore" },
                    { 39, true, "Leather" },
                    { 40, true, "Lily" },
                    { 41, true, "Mammoth Tusk" },
                    { 42, false, "Organic Resin" },
                    { 43, true, "Oxite" },
                    { 44, false, "Platinum Ingot" },
                    { 45, true, "Platinum Ore" },
                    { 46, false, "Poison Paste" },
                    { 47, true, "Poison Sack" },
                    { 48, true, "Polar Bear Pelt" },
                    { 49, true, "Raw Meat" },
                    { 50, true, "Raw Prime Meat" },
                    { 51, true, "Reed Flower" },
                    { 52, false, "Refined Gold" },
                    { 53, false, "Refined Wood" },
                    { 54, false, "Rope" },
                    { 55, true, "Salt" },
                    { 56, true, "Sandworm Scale" },
                    { 57, true, "Scorpion Pincer" },
                    { 58, true, "Scorpion Tail" },
                    { 59, true, "Silica Ore" },
                    { 60, true, "Spoiled Meat" },
                    { 61, true, "Spoiled Plants" },
                    { 62, true, "Sponge" },
                    { 63, true, "Squash" },
                    { 64, false, "Steel Bloom" },
                    { 65, false, "Steel Ingot" },
                    { 66, false, "Steel Rebar" },
                    { 67, false, "Steel Screw" },
                    { 68, false, "Stick" },
                    { 69, true, "Stone" },
                    { 70, true, "Sulfur" },
                    { 71, true, "Tea" },
                    { 72, false, "Titanium Ingot" },
                    { 73, true, "Titanium Ore" },
                    { 74, false, "Tree Sap" },
                    { 75, true, "Wheat" },
                    { 76, true, "Wild Berry" },
                    { 77, true, "Wood" },
                    { 78, true, "Worm Scale" },
                    { 79, true, "Yeast" },
                    { 80, false, "Crafting Bench" },
                    { 81, false, "Anvil Bench" },
                    { 82, false, "Wood Composter" },
                    { 83, false, "Skinning Bench" },
                    { 84, false, "Trophy Bench" },
                    { 85, false, "Basic Fishing Bench" },
                    { 86, false, "Mortar and Pestle" },
                    { 87, false, "Herbalism Bench" },
                    { 88, false, "Textiles Bench" },
                    { 89, false, "Carpentry Bench" },
                    { 90, false, "Masonry Bench" },
                    { 91, false, "Machining Bench" },
                    { 92, false, "Cement Mixer" },
                    { 93, false, "Glassworking Bench" },
                    { 94, false, "Advanced Textiles Bench" },
                    { 95, false, "Alteration Bench" },
                    { 96, false, "Fabricator" },
                    { 97, false, "Material Processor" },
                    { 98, false, "Chemistry Bench" },
                    { 99, false, "Electric Textiles Bench" },
                    { 100, false, "Advanced Alteration Bench" },
                    { 101, false, "Electric Carpentry Bench" },
                    { 102, false, "Electric Masonry Bench" },
                    { 103, false, "Stone Furnace" },
                    { 104, false, "Concrete Furnace" },
                    { 105, false, "Electric Furnace" },
                    { 106, false, "MXC Furnace" },
                    { 107, false, "Campfire" },
                    { 108, false, "Drying Rack" },
                    { 109, false, "Firepit" },
                    { 110, false, "Fireplace" },
                    { 111, false, "Cooking Station" },
                    { 112, false, "Potbelly Stove" },
                    { 113, false, "Kitchen Bench" },
                    { 114, false, "Biofuel Stove" },
                    { 115, false, "Marble Kitchen Bench" },
                    { 116, false, "Electric Stove" },
                    { 117, false, "MXC Campfire" },
                    { 118, false, "Salting Station" },
                    { 119, false, "Ice Box" },
                    { 120, false, "Refrigerator" },
                    { 121, false, "Deep Freeze" },
                    { 122, false, "Wood Pile" },
                    { 123, false, "Stone Pile" },
                    { 124, false, "Oxidizer" },
                    { 125, false, "Oxite Dissolver" },
                    { 126, false, "Biofuel Oxite Dissolver" },
                    { 127, false, "Dehumidifier" },
                    { 128, false, "Electric Dehumidifier" },
                    { 129, false, "Floor Torch" },
                    { 130, false, "Brazier" },
                    { 131, false, "Wall Torch" },
                    { 132, false, "Basic Wall Light" },
                    { 133, false, "Basic Ceiling Light" },
                    { 134, false, "Directional Worklamp" },
                    { 135, false, "Omnidirectional Worklamp" },
                    { 136, false, "Heavy Heater" },
                    { 137, false, "Heavy Air Conditioner" },
                    { 138, false, "Rain Reservoir" },
                    { 139, false, "Electric Water Pump" },
                    { 140, false, "Biofuel Composter" },
                    { 141, false, "Electric Composter" },
                    { 142, false, "Biofuel Generator" },
                    { 143, false, "Solar Panel" },
                    { 144, false, "Water Wheel" },
                    { 145, false, "Wind Turbine" },
                    { 146, false, "Portable Biofuel Generator" },
                    { 147, false, "Biofuel Radar" },
                    { 148, false, "Biofuel Extractor" },
                    { 149, false, "Electric Radar" },
                    { 150, false, "Electric Extractor" },
                    { 151, false, "Medium Wood Hedgehog" },
                    { 152, false, "Wood Gate Fortification" },
                    { 153, false, "Wood Wall Fortification" },
                    { 154, false, "Wood Spikes Fortification" },
                    { 155, false, "Lightning Rod" },
                    { 156, false, "Water Sprinkler" },
                    { 157, false, "Scorpion Pincer Trap" },
                    { 158, false, "Scorpion Hedgehog" },
                    { 159, false, "Wood Crop Plot" },
                    { 160, false, "Iron Crop Plot" },
                    { 161, false, "Hydroponic Crop Plot" },
                    { 162, false, "Animal Bait" },
                    { 163, false, "Poisoned Animal Bait" },
                    { 164, false, "Titanium Plate" },
                    { 165, false, "Flow Meter" },
                    { 166, false, "Biofuel Deep-Mining Drill" },
                    { 167, false, "Electric Deep-Mining Drill" },
                    { 168, false, "Animal Fat" },
                    { 169, false, "Electrolytic Oxygen Synthesizer" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "RecipeId", "ItemId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 5 },
                    { 6, 6 },
                    { 11, 11 },
                    { 12, 12 },
                    { 13, 13 },
                    { 14, 14 },
                    { 15, 15 },
                    { 17, 17 },
                    { 19, 19 },
                    { 21, 21 },
                    { 22, 22 },
                    { 26, 26 },
                    { 28, 28 },
                    { 30, 30 },
                    { 32, 32 },
                    { 33, 33 },
                    { 34, 34 },
                    { 36, 36 },
                    { 37, 37 },
                    { 42, 42 },
                    { 44, 44 },
                    { 46, 46 },
                    { 52, 52 },
                    { 53, 53 },
                    { 54, 54 },
                    { 64, 64 },
                    { 65, 65 },
                    { 66, 66 },
                    { 67, 67 },
                    { 72, 72 },
                    { 74, 74 },
                    { 80, 80 },
                    { 81, 81 },
                    { 82, 82 },
                    { 83, 83 },
                    { 84, 84 },
                    { 85, 85 },
                    { 86, 86 },
                    { 87, 87 },
                    { 88, 88 },
                    { 89, 89 },
                    { 90, 90 },
                    { 91, 91 },
                    { 92, 92 },
                    { 93, 93 },
                    { 94, 94 },
                    { 95, 95 },
                    { 96, 96 },
                    { 97, 97 },
                    { 98, 98 },
                    { 99, 99 },
                    { 100, 100 },
                    { 101, 101 },
                    { 102, 102 },
                    { 103, 103 },
                    { 104, 104 },
                    { 105, 105 },
                    { 107, 107 },
                    { 108, 108 },
                    { 109, 109 },
                    { 110, 110 },
                    { 111, 111 },
                    { 112, 112 },
                    { 113, 113 },
                    { 114, 114 },
                    { 115, 115 },
                    { 116, 116 },
                    { 118, 118 },
                    { 119, 119 },
                    { 120, 120 },
                    { 121, 121 },
                    { 122, 122 },
                    { 123, 123 },
                    { 124, 124 },
                    { 125, 125 },
                    { 126, 126 },
                    { 127, 127 },
                    { 128, 128 },
                    { 129, 129 },
                    { 130, 130 },
                    { 131, 131 },
                    { 132, 132 },
                    { 133, 133 },
                    { 134, 134 },
                    { 135, 135 },
                    { 136, 136 },
                    { 137, 137 },
                    { 138, 138 },
                    { 139, 139 },
                    { 140, 140 },
                    { 141, 141 },
                    { 142, 142 },
                    { 143, 143 },
                    { 144, 144 },
                    { 146, 146 },
                    { 147, 147 },
                    { 148, 148 },
                    { 149, 149 },
                    { 150, 150 },
                    { 151, 151 },
                    { 152, 152 },
                    { 153, 153 },
                    { 154, 154 },
                    { 155, 155 },
                    { 156, 156 },
                    { 157, 157 },
                    { 158, 158 },
                    { 159, 159 },
                    { 160, 160 },
                    { 161, 161 },
                    { 162, 162 },
                    { 163, 163 },
                    { 164, 164 },
                    { 165, 165 },
                    { 166, 166 },
                    { 167, 167 },
                    { 168, 168 },
                    { 169, 169 }
                });

            migrationBuilder.InsertData(
                table: "RecipeItems",
                columns: new[] { "RecipeItemId", "ItemId", "Quantity", "RecipeId" },
                values: new object[,]
                {
                    { 1, 2, 1.0, 1 },
                    { 2, 61, 10.0, 3 },
                    { 3, 20, 10.0, 3 },
                    { 4, 6, 1.0, 5 },
                    { 5, 59, 1.0, 6 },
                    { 6, 1, 1.0, 6 },
                    { 7, 22, 2.0, 6 },
                    { 8, 42, 4.0, 6 },
                    { 9, 59, 1.0, 11 },
                    { 10, 38, 2.0, 11 },
                    { 11, 29, 1.0, 11 },
                    { 12, 42, 1.0, 11 },
                    { 13, 11, 1.0, 12 },
                    { 14, 69, 8.0, 13 },
                    { 15, 59, 4.0, 13 },
                    { 16, 74, 1.0, 13 },
                    { 17, 16, 2.0, 14 },
                    { 18, 14, 1.0, 15 },
                    { 19, 14, 0.20000000000000001, 17 },
                    { 20, 4, 2.0, 19 },
                    { 21, 22, 2.0, 21 },
                    { 22, 42, 2.0, 21 },
                    { 23, 17, 15.0, 21 },
                    { 24, 30, 5.0, 21 },
                    { 25, 70, 2.0, 22 },
                    { 26, 74, 4.0, 22 },
                    { 27, 75, 10.0, 26 },
                    { 28, 59, 1.0, 28 },
                    { 29, 52, 0.20000000000000001, 30 },
                    { 30, 59, 5.0, 32 },
                    { 31, 3, 1.0, 32 },
                    { 32, 70, 1.0, 33 },
                    { 33, 7, 3.0, 33 },
                    { 34, 70, 5.0, 34 },
                    { 35, 3, 1.0, 34 },
                    { 36, 38, 2.0, 36 },
                    { 37, 36, 0.10000000000000001, 37 },
                    { 38, 77, 1.0, 42 },
                    { 39, 43, 1.0, 42 },
                    { 40, 45, 5.0, 44 },
                    { 41, 60, 2.0, 46 },
                    { 42, 70, 1.0, 46 },
                    { 43, 29, 2.0, 52 },
                    { 44, 77, 0.20000000000000001, 53 },
                    { 45, 24, 12.0, 54 },
                    { 46, 8, 1.0, 64 },
                    { 47, 38, 6.0, 64 },
                    { 48, 64, 1.0, 65 },
                    { 49, 65, 0.10000000000000001, 66 },
                    { 50, 65, 0.01, 67 },
                    { 51, 68, 4.0, 74 },
                    { 52, 73, 5.0, 72 },
                    { 54, 24, 60.0, 80 },
                    { 55, 39, 20.0, 80 },
                    { 56, 69, 12.0, 80 },
                    { 57, 77, 50.0, 80 },
                    { 58, 36, 40.0, 81 },
                    { 59, 69, 10.0, 81 },
                    { 60, 77, 20.0, 81 },
                    { 61, 61, 5.0, 82 },
                    { 62, 69, 10.0, 82 },
                    { 63, 70, 5.0, 82 },
                    { 64, 77, 25.0, 82 },
                    { 65, 24, 60.0, 83 },
                    { 66, 39, 20.0, 83 },
                    { 67, 69, 12.0, 83 },
                    { 68, 77, 50.0, 83 },
                    { 69, 15, 50.0, 84 },
                    { 70, 36, 2.0, 84 },
                    { 71, 54, 12.0, 84 },
                    { 72, 77, 60.0, 84 },
                    { 73, 15, 12.0, 85 },
                    { 74, 36, 5.0, 85 },
                    { 75, 54, 10.0, 85 },
                    { 76, 77, 25.0, 85 },
                    { 77, 59, 4.0, 86 },
                    { 78, 69, 12.0, 86 },
                    { 79, 24, 12.0, 87 },
                    { 80, 68, 20.0, 87 },
                    { 81, 69, 12.0, 87 },
                    { 82, 77, 50.0, 87 },
                    { 83, 24, 60.0, 88 },
                    { 84, 68, 20.0, 88 },
                    { 85, 69, 12.0, 88 },
                    { 86, 77, 50.0, 88 },
                    { 87, 15, 120.0, 89 },
                    { 88, 54, 12.0, 89 },
                    { 89, 77, 80.0, 89 },
                    { 90, 37, 120.0, 90 },
                    { 91, 39, 12.0, 90 },
                    { 92, 54, 12.0, 90 },
                    { 93, 77, 80.0, 90 },
                    { 94, 22, 10.0, 91 },
                    { 95, 36, 40.0, 91 },
                    { 96, 37, 120.0, 91 },
                    { 97, 54, 24.0, 91 },
                    { 98, 69, 12.0, 91 },
                    { 99, 77, 20.0, 91 },
                    { 100, 36, 20.0, 92 },
                    { 101, 37, 8.0, 92 },
                    { 102, 53, 50.0, 92 },
                    { 103, 54, 8.0, 92 },
                    { 104, 69, 40.0, 92 },
                    { 105, 15, 40.0, 93 },
                    { 106, 22, 6.0, 93 },
                    { 107, 36, 24.0, 93 },
                    { 108, 53, 10.0, 93 },
                    { 109, 54, 6.0, 93 },
                    { 110, 22, 8.0, 94 },
                    { 111, 36, 20.0, 94 },
                    { 112, 54, 4.0, 94 },
                    { 113, 67, 40.0, 94 },
                    { 114, 77, 6.0, 94 },
                    { 115, 15, 40.0, 95 },
                    { 116, 22, 6.0, 95 },
                    { 117, 36, 24.0, 95 },
                    { 118, 53, 10.0, 95 },
                    { 119, 54, 6.0, 95 },
                    { 120, 1, 40.0, 96 },
                    { 121, 5, 8.0, 96 },
                    { 122, 13, 30.0, 96 },
                    { 123, 21, 30.0, 96 },
                    { 124, 67, 30.0, 96 },
                    { 130, 1, 30.0, 97 },
                    { 131, 5, 8.0, 97 },
                    { 132, 21, 60.0, 97 },
                    { 133, 67, 12.0, 97 },
                    { 134, 164, 3.0, 97 },
                    { 135, 72, 3.0, 164 },
                    { 136, 12, 10.0, 98 },
                    { 137, 21, 20.0, 98 },
                    { 138, 28, 10.0, 98 },
                    { 139, 53, 30.0, 98 },
                    { 140, 65, 20.0, 98 },
                    { 141, 13, 15.0, 99 },
                    { 142, 21, 8.0, 99 },
                    { 143, 65, 30.0, 99 },
                    { 144, 67, 18.0, 99 },
                    { 145, 13, 25.0, 100 },
                    { 146, 21, 10.0, 100 },
                    { 147, 28, 4.0, 100 },
                    { 148, 65, 30.0, 100 },
                    { 149, 67, 8.0, 100 },
                    { 150, 12, 10.0, 101 },
                    { 151, 21, 20.0, 101 },
                    { 152, 36, 40.0, 101 },
                    { 153, 65, 60.0, 101 },
                    { 154, 67, 10.0, 101 },
                    { 155, 12, 10.0, 102 },
                    { 156, 13, 100.0, 102 },
                    { 157, 21, 20.0, 102 },
                    { 158, 65, 60.0, 102 },
                    { 159, 67, 10.0, 102 },
                    { 160, 39, 12.0, 103 },
                    { 161, 68, 4.0, 103 },
                    { 162, 69, 80.0, 103 },
                    { 163, 77, 12.0, 103 },
                    { 164, 13, 20.0, 104 },
                    { 165, 22, 12.0, 104 },
                    { 166, 36, 12.0, 104 },
                    { 167, 54, 8.0, 104 },
                    { 168, 13, 80.0, 105 },
                    { 169, 21, 60.0, 105 },
                    { 170, 28, 4.0, 105 },
                    { 171, 65, 30.0, 105 },
                    { 172, 67, 8.0, 105 },
                    { 173, 24, 8.0, 107 },
                    { 174, 68, 8.0, 107 },
                    { 175, 69, 24.0, 107 },
                    { 176, 24, 50.0, 108 },
                    { 177, 68, 25.0, 108 },
                    { 178, 77, 50.0, 108 },
                    { 179, 69, 100.0, 109 },
                    { 180, 77, 25.0, 109 },
                    { 181, 69, 120.0, 110 },
                    { 182, 77, 30.0, 110 },
                    { 183, 24, 8.0, 111 },
                    { 184, 36, 4.0, 111 },
                    { 185, 68, 8.0, 111 },
                    { 186, 69, 24.0, 111 },
                    { 187, 14, 8.0, 112 },
                    { 188, 22, 10.0, 112 },
                    { 189, 36, 40.0, 112 },
                    { 190, 15, 20.0, 113 },
                    { 191, 22, 20.0, 113 },
                    { 192, 36, 8.0, 113 },
                    { 193, 53, 30.0, 113 },
                    { 194, 15, 4.0, 114 },
                    { 195, 22, 20.0, 114 },
                    { 196, 53, 30.0, 114 },
                    { 197, 65, 24.0, 114 },
                    { 198, 67, 8.0, 114 },
                    { 199, 1, 20.0, 115 },
                    { 200, 12, 2.0, 115 },
                    { 201, 13, 10.0, 115 },
                    { 202, 67, 8.0, 115 },
                    { 203, 69, 100.0, 115 },
                    { 204, 1, 20.0, 116 },
                    { 205, 12, 10.0, 116 },
                    { 206, 17, 8.0, 116 },
                    { 207, 21, 10.0, 116 },
                    { 208, 28, 12.0, 116 },
                    { 209, 67, 8.0, 116 },
                    { 210, 7, 25.0, 118 },
                    { 211, 24, 60.0, 118 },
                    { 212, 69, 25.0, 118 },
                    { 213, 77, 50.0, 118 },
                    { 214, 15, 4.0, 119 },
                    { 215, 36, 8.0, 119 },
                    { 216, 39, 24.0, 119 },
                    { 217, 54, 8.0, 119 },
                    { 218, 77, 40.0, 119 },
                    { 219, 1, 40.0, 120 },
                    { 220, 5, 8.0, 120 },
                    { 221, 21, 20.0, 120 },
                    { 222, 67, 12.0, 120 },
                    { 223, 1, 20.0, 121 },
                    { 224, 5, 10.0, 121 },
                    { 225, 21, 15.0, 121 },
                    { 226, 44, 20.0, 121 },
                    { 227, 67, 15.0, 121 },
                    { 228, 77, 100.0, 122 },
                    { 229, 69, 100.0, 123 },
                    { 230, 4, 10.0, 124 },
                    { 231, 24, 24.0, 124 },
                    { 232, 39, 20.0, 124 },
                    { 233, 68, 8.0, 124 },
                    { 234, 4, 8.0, 125 },
                    { 235, 39, 24.0, 125 },
                    { 236, 77, 18.0, 125 },
                    { 242, 17, 50.0, 126 },
                    { 243, 21, 20.0, 126 },
                    { 244, 22, 12.0, 126 },
                    { 245, 28, 4.0, 126 },
                    { 246, 65, 15.0, 126 },
                    { 247, 17, 50.0, 127 },
                    { 248, 21, 20.0, 127 },
                    { 249, 22, 12.0, 127 },
                    { 250, 28, 4.0, 127 },
                    { 251, 65, 15.0, 127 },
                    { 252, 1, 20.0, 128 },
                    { 253, 12, 20.0, 128 },
                    { 254, 21, 35.0, 128 },
                    { 255, 28, 15.0, 128 },
                    { 256, 67, 12.0, 128 },
                    { 257, 24, 20.0, 129 },
                    { 258, 68, 8.0, 129 },
                    { 259, 70, 8.0, 129 },
                    { 260, 14, 4.0, 130 },
                    { 261, 36, 20.0, 130 },
                    { 267, 1, 6.0, 165 },
                    { 268, 17, 10.0, 165 },
                    { 269, 21, 2.0, 165 },
                    { 270, 28, 2.0, 165 },
                    { 271, 30, 10.0, 165 },
                    { 272, 5, 8.0, 143 },
                    { 273, 12, 18.0, 143 },
                    { 274, 21, 30.0, 143 },
                    { 275, 28, 60.0, 143 },
                    { 276, 67, 10.0, 143 },
                    { 277, 14, 10.0, 166 },
                    { 278, 17, 50.0, 166 },
                    { 279, 21, 10.0, 166 },
                    { 280, 22, 12.0, 166 },
                    { 281, 65, 12.0, 166 },
                    { 282, 1, 25.0, 167 },
                    { 283, 5, 8.0, 167 },
                    { 284, 21, 15.0, 167 },
                    { 285, 67, 12.0, 167 },
                    { 286, 72, 8.0, 167 },
                    { 287, 36, 8.0, 131 },
                    { 288, 39, 8.0, 131 },
                    { 289, 70, 8.0, 131 },
                    { 290, 77, 4.0, 131 },
                    { 291, 17, 40.0, 132 },
                    { 292, 28, 6.0, 132 },
                    { 293, 36, 15.0, 132 },
                    { 294, 17, 40.0, 133 },
                    { 295, 28, 6.0, 133 },
                    { 296, 36, 15.0, 133 },
                    { 297, 1, 10.0, 134 },
                    { 298, 12, 5.0, 134 },
                    { 299, 21, 2.0, 134 },
                    { 300, 28, 5.0, 134 },
                    { 301, 1, 20.0, 135 },
                    { 302, 12, 10.0, 135 },
                    { 303, 21, 2.0, 135 },
                    { 304, 28, 5.0, 135 },
                    { 305, 1, 40.0, 136 },
                    { 306, 5, 8.0, 136 },
                    { 307, 21, 20.0, 136 },
                    { 308, 67, 8.0, 136 },
                    { 309, 1, 40.0, 137 },
                    { 310, 5, 8.0, 137 },
                    { 311, 21, 20.0, 137 },
                    { 312, 67, 8.0, 137 },
                    { 313, 39, 8.0, 138 },
                    { 314, 68, 8.0, 138 },
                    { 315, 69, 60.0, 138 },
                    { 316, 77, 24.0, 138 },
                    { 317, 13, 20.0, 139 },
                    { 318, 21, 4.0, 139 },
                    { 319, 65, 24.0, 139 },
                    { 320, 67, 16.0, 139 },
                    { 321, 13, 12.0, 140 },
                    { 322, 14, 8.0, 140 },
                    { 323, 36, 20.0, 140 },
                    { 324, 67, 20.0, 140 },
                    { 325, 12, 20.0, 141 },
                    { 326, 13, 100.0, 141 },
                    { 327, 21, 20.0, 141 },
                    { 328, 65, 60.0, 141 },
                    { 329, 67, 20.0, 141 },
                    { 330, 17, 40.0, 142 },
                    { 331, 21, 12.0, 142 },
                    { 332, 28, 2.0, 142 },
                    { 333, 65, 20.0, 142 },
                    { 334, 67, 20.0, 142 },
                    { 335, 15, 50.0, 144 },
                    { 336, 21, 10.0, 144 },
                    { 337, 42, 25.0, 144 },
                    { 338, 65, 10.0, 144 },
                    { 339, 69, 50.0, 144 },
                    { 340, 77, 100.0, 144 },
                    { 341, 17, 50.0, 147 },
                    { 342, 28, 2.0, 147 },
                    { 343, 30, 50.0, 147 },
                    { 344, 36, 12.0, 147 },
                    { 345, 37, 8.0, 147 },
                    { 346, 15, 10.0, 148 },
                    { 347, 30, 25.0, 148 },
                    { 348, 36, 15.0, 148 },
                    { 349, 12, 12.0, 149 },
                    { 350, 21, 10.0, 149 },
                    { 351, 28, 2.0, 149 },
                    { 352, 65, 12.0, 149 },
                    { 353, 67, 8.0, 149 },
                    { 354, 21, 5.0, 150 },
                    { 355, 36, 20.0, 150 },
                    { 357, 54, 4.0, 151 },
                    { 358, 77, 60.0, 151 },
                    { 359, 36, 6.0, 152 },
                    { 360, 54, 4.0, 152 },
                    { 361, 77, 40.0, 152 },
                    { 362, 37, 18.0, 153 },
                    { 363, 54, 12.0, 153 },
                    { 364, 77, 50.0, 153 },
                    { 365, 54, 6.0, 154 },
                    { 366, 77, 80.0, 154 },
                    { 367, 14, 10.0, 155 },
                    { 368, 17, 4.0, 156 },
                    { 369, 22, 4.0, 156 },
                    { 370, 65, 2.0, 156 },
                    { 371, 57, 1.0, 157 },
                    { 372, 68, 4.0, 157 },
                    { 373, 69, 4.0, 157 },
                    { 374, 58, 1.0, 158 },
                    { 375, 151, 1.0, 158 },
                    { 376, 20, 20.0, 159 },
                    { 377, 70, 10.0, 159 },
                    { 378, 77, 8.0, 159 },
                    { 379, 17, 20.0, 160 },
                    { 380, 20, 20.0, 160 },
                    { 381, 36, 10.0, 160 },
                    { 382, 67, 8.0, 160 },
                    { 383, 70, 12.0, 160 },
                    { 384, 12, 20.0, 161 },
                    { 385, 17, 20.0, 161 },
                    { 386, 20, 20.0, 161 },
                    { 387, 21, 10.0, 161 },
                    { 388, 65, 10.0, 161 },
                    { 389, 67, 25.0, 161 },
                    { 390, 49, 1.0, 168 },
                    { 391, 49, 1.0, 162 },
                    { 392, 168, 2.0, 162 },
                    { 393, 46, 2.0, 163 },
                    { 394, 162, 1.0, 163 },
                    { 395, 17, 20.0, 146 },
                    { 396, 21, 6.0, 146 },
                    { 397, 28, 2.0, 146 },
                    { 398, 65, 10.0, 146 },
                    { 399, 67, 10.0, 146 },
                    { 400, 17, 70.0, 169 },
                    { 401, 21, 20.0, 169 },
                    { 402, 28, 10.0, 169 },
                    { 403, 164, 2.0, 169 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_ItemId",
                table: "RecipeItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_RecipeId",
                table: "RecipeItems",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ItemId",
                table: "Recipes",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RecipeItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
