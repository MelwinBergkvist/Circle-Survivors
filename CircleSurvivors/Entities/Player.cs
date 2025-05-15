using CircleSurvivors.Core;
using CircleSurvivors.Interfaces;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CircleSurvivors.Entities
{
    /// <summary>
    /// hanterar playern
    /// </summary>
    public class Player : IDrawable
    {
        Vector2 velocity = new Vector2(0, 0);
        public float x, y;
        public int movementSpeed = 100;
        float turnSpeed = 10f;
        float friction = 3f;
        public int radius = 15;
        public int healthpoints = 100;
        public int maxHealthpoints = 100;
        float bulletCooldownTimer = 0f;

        /// <summary>
        /// specifiserar början av x, y för player
        /// </summary>
        /// <param name="x">player initial X</param>
        /// <param name="y">player initial Y</param>
        public Player(int x, int y) //constructor
        {
            //specificerar att det är x och y från parametrarna
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Ritar spelaren och Health Baren
        /// </summary>
        public void Draw()
        {
            //spelaren
            Raylib.DrawCircle((int)x, (int)y, Config.player.radius, Color.Green);

            //health bar
            Raylib.DrawRectangle((int)x - 15, (int)y + 25, 30, 8, Color.Green);
            float healthWidth = 30 - 30 * healthpoints / maxHealthpoints;
            Raylib.DrawRectangle((int)x - 15, (int)y + 25, (int)healthWidth, 8, Color.Red);
        }
        /// <summary>
        /// Kollar om spelaren ska despawna
        /// </summary>
        /// <returns>player alive status</returns>
        public bool ShouldDespawn()
        {
            return healthpoints <= 0;
        }
        /// <summary>
        /// hanterar movement, canvas restrictions, och bullet shootin mechanics.
        /// </summary>
        /// <param name="deltaTime">tid</param>
        public void Update(float deltaTime) //kallar över deltatime som ett argument
        {
            ShootBullet(deltaTime);
            VectorMovement(deltaTime);
            CanvasBorder();
        }
        /// <summary>
        /// kollar distancen mellan alla enemies och spelaren, används för enemies listan sortering
        /// </summary>
        /// <param name="player">spelaren</param>
        /// <param name="npc">enemies</param>
        /// <returns>distancen mellan enemies och spelaren</returns>
        public static float EnemyDistance(Player player, NPC npc)
        {
            //Hur jag definerar i vilken ordning enemies ska vara sorted, räknar delta x och y
            float dx = npc.x - player.x;
            float dy = npc.y - player.y;
            return dx * dx + dy * dy;
        }
        /// <summary>
        /// sorterar alla enemies som är spawnade från närmaste till längst bort
        /// </summary>
        /// <param name="enemies">enemies</param>
        /// <returns>närmaste enemy</returns>
        public static NPC ClosestEnemy(List<NPC> enemies)
        {
            //enemies = enemies.OrderBy(enemy => EnemyDistance(Config.player, enemy)).ToList();    <- min originella sortering
            enemies.Sort((a, b) => (int)(EnemyDistance(Config.player, a) - EnemyDistance(Config.player, b)));
            //brorsan hjälpa mig att skriva den och förklara att min tidigare .ToList tappade bort referensen
            //därför fungerade inte enemies.Clear();,
            //den nya listan fungerar genom att modifiera existerande listan utan att skriva över den originella
            return enemies[0];
        }
        /// <summary>
        /// ser till att spelaren inte lämnar canvasen
        /// </summary>
        public void CanvasBorder()
        {
            //Player kan inte lämna Canvas
            if (x - radius <= 0) x = radius;
            if (x + radius >= Config.WindowSizeWidth) x = Config.WindowSizeWidth - radius;
            if (y - radius <= 0) y = radius;
            if (y + radius >= Config.WindowSizeHeight) y = Config.WindowSizeHeight - radius;
        }
        /// <summary>
        /// updaterar movements för spelaren
        /// </summary>
        /// <param name="deltaTime"></param>
        public void VectorMovement(float deltaTime)
        {
            //!!!Jag förklarar vector matten och vad Raymath gör i NPC.cs NpcMovements!!!

            Vector2 direction = new Vector2(0, 0);
            //bara if och inte if else för att vi vill att Playern ska kunna gå diagonalt
            if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up))
                direction.Y -= 1;
            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
                direction.X -= 1;
            if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
                direction.Y += 1;
            if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
                direction.X += 1;

            if (direction.X != 0 || direction.Y != 0)
            {
                Vector2 endVelocity = Raymath.Vector2Scale(direction, movementSpeed);
                velocity = Raymath.Vector2Lerp(velocity, endVelocity, turnSpeed * deltaTime);
            }
            else
            {
                velocity = Raymath.Vector2Lerp(velocity, new Vector2(0, 0), friction * deltaTime);
            }

            x += velocity.X * deltaTime;
            y += velocity.Y * deltaTime;
        }
        /// <summary>
        /// skjuter bullets för spelaren
        /// </summary>
        /// <param name="deltaTime"></param>
        public void ShootBullet(float deltaTime)
        {
            //plussar på deltatime tills cooldown timern är större en cooldownen
            //när det händer ressettar vi timern och skapar en bullet i drawablelist och bullets
            bulletCooldownTimer += deltaTime;
            if (Config.bulletCooldown <= bulletCooldownTimer)
            {
                //make sure att det faktist finns enemies på skärmen,
                //så vi inte försöker skjuta mot något som inte finns
                //annars går spelet kapput, crashar
                if (Config.enemiesList.Count > 0)
                {
                    bulletCooldownTimer = 0;
                    BaseAbility bullet = new BaseAbility(ClosestEnemy(Config.enemiesList));
                    Config.drawableList.Add(bullet);
                    Config.bulletsList.Add(bullet);
                }
            }
        }
    }
}
