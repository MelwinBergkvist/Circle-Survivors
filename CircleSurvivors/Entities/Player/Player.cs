using CircleSurvivors.Entities.Enemies;
using CircleSurvivors.Interfaces;
using CircleSurvivors.Core;
using System.Numerics;
using Raylib_cs;
using CircleSurvivors.UI_Helpers;

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
        const float turnSpeed = 10f;
        const float friction = 3f;

        double cos = 0;
        double radiusAimer = 40;

        //stats
        public int movementSpeed = 100;
        public int radius = 15;
        public int healthpoints = 100;
        public int maxHealthpoints = 100;
        public int dashSpeed = 3;

        //timers
        float bulletCooldownTimer = 0f;
        public float dashDuration = 0.5f;
        public float maxDashDuration = 0.5f;
        public float dashRegain = 10;
        float aimerCooldown = 0.5f;

        //states
        bool isDashing = false;
        bool canDash = false;
        bool startDash = false;
        bool isMoving = false;

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

            //dash bar
            Raylib.DrawRectangle((int)x - 15, (int)y + 20, 30, 4, new(46, 241, 255));
            float dashWidth = 30 - 30 * dashDuration / maxDashDuration;
            if (dashWidth >= 30) dashWidth = 30; //ser till att rektangeln inte blir för lång
            Raylib.DrawRectangle((int)x - 15, (int)y + 20, (int)dashWidth, 4, new(18, 69, 255));

            //Direction pointer
            Raylib.DrawCircle((int)x + (int)velocity.X/2, (int)y + (int)velocity.Y/2, 2, Color.Blue);
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
            Aim(deltaTime);
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
            return Helper.EuclideanFloat(ref player.x, ref npc.x, ref player.y, ref npc.y).distance;
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

            if (direction.X != 0 || direction.Y != 0)
            {
                Vector2 endVelocity = Raymath.Vector2Scale(direction, movementSpeed);
                velocity = Raymath.Vector2Lerp(velocity, endVelocity, turnSpeed * deltaTime);
                isMoving = true;
            }
            else
            {
                isMoving = false;
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

            //ser till att det inte blir någon partial dashing eller ingen dashing alls utan movement
            if (Raylib.IsKeyPressed(KeyboardKey.Space) && canDash && isMoving)
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
                if (dashDuration < maxDashDuration)
                {
                    dashDuration += deltaTime / dashRegain;
                }
                return;
            }

            if (Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up))
                direction.Y -= dashSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left))
                direction.X -= dashSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down))
                direction.Y += dashSpeed;
            if (Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right))
                direction.X += dashSpeed;
        }
        public void Aim(float deltaTime)
        {
            double pointerX = x + radiusAimer * Math.Cos(cos);
            double pointerY = y + radiusAimer* Math.Sin(cos);

            if (aimerCooldown > 0)
            {
                aimerCooldown -= deltaTime;
            }
            if (aimerCooldown < 0)
            {
                aimerCooldown = 0.01f;
                cos += 0.1f;
            }

            Raylib.DrawCircle((int)pointerX, (int)pointerY, 3, Color.Black);

        }
    }
}
