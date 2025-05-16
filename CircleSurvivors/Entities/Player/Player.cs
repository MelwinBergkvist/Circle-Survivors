using CircleSurvivors.Entities.Enemies;
using CircleSurvivors.Interfaces;
using CircleSurvivors.Core;
using System.Numerics;
using Raylib_cs;

namespace CircleSurvivors.Entities.Player
{
    /// <summary>
    /// hanterar playern
    /// </summary>
    public class Player : IDrawable //implementerar drawable interface
    {
        //positioner
        Vector2 velocity = new Vector2(0, 0);
        Vector2 direction;
        public float x, y;
        float turnSpeed = 10f;
        float friction = 3f;

        //stats
        public int movementSpeed = 100;
        public int radius = 15;
        public int healthpoints = 100;
        public int maxHealthpoints = 100;

        //timers
        float bulletCooldownTimer = 0f;
        float dashDuration = 0.5f;
        int dashRegain = 10;

        //states
        bool isDashing = false;
        bool canDash = false;
        bool startDash = false;

        /// <summary>
        /// specifiserar början av x, y för player
        /// </summary>
        /// <param name="x">player initial X</param>
        /// <param name="y">player initial Y</param>
        public Player(int x, int y)
        {
            //specificerar att det är x och y inom parametrarna
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

            direction = new Vector2(0, 0);
            
            DashMovement(deltaTime);
            NormalMovement();

            Raylib.DrawText($"{dashDuration}", 20, 250, 16, Color.Black);
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
        /// <summary>
        /// vanlig movement logik utan dash aktiv
        /// </summary>
        public void NormalMovement()
        {
            if (isDashing) return;
            //bara if och inte if else för att vi vill att Playern ska kunna gå diagonalt
            if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up))
                direction.Y -= 1;
            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
                direction.X -= 1;
            if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
                direction.Y += 1;
            if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
                direction.X += 1;
        }
        /// <summary>
        /// movement för dashing
        /// </summary>
        /// <param name="deltaTime"></param>
        public void DashMovement(float deltaTime)
        {
            //ser till att vi kan bara börja en dash när duration är full
            if (dashDuration >= 0.5f)
            {
                canDash = true;
            }
            else if (dashDuration <= 0)
            {
                canDash = false;
                startDash = false;
            }

            //ser till att det inte blir någon partial dashing
            if (Raylib.IsKeyPressed(KeyboardKey.Space) && canDash)
            {
                startDash = true;
            }

            //om vi kan dasha och har tryckt space så dashar vi
            if (startDash && canDash)
            {
                if (dashDuration > 0)
                {
                    isDashing = true;
                    dashDuration -= deltaTime;
                }
                if (dashDuration <= 0)
                {
                    isDashing = false;
                    dashDuration = -0.1f;
                    return;
                }
            }
            //annars regainar vi bara våran dash duration
            else
            {
                isDashing = false;
                if (dashDuration < 0.5f)
                {
                    dashDuration += deltaTime / dashRegain;
                }
                return;
            }

            if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up))
                direction.Y -= 3;
            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
                direction.X -= 3;
            if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
                direction.Y += 3;
            if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
                direction.X += 3;
        }
    }
}
