﻿using AAI_Final_Assignment_WinForms.Behaviour;
using AAI_Final_Assignment_WinForms.util;
using AAI_Final_Assignment_WinForms.World;

namespace AAI_Final_Assignment_WinForms.Entities; 

public abstract class MovingEntity : BaseGameEntity {
    // used for debugging to show current steeringforce
    private Vector2D currentSteeringForce;

    protected MovingEntity(Vector2D pos, GameWorld world, float scale, int textureWidth, int textureHeight,
        float mass, float maxSpeed, float maxForce, float radius
    ) : base(pos, world, scale, textureWidth, textureHeight, radius) {
        Mass = mass;
        MaxSpeed = maxSpeed;
        MaxForce = maxForce;
        Velocity = new Vector2D();
        Heading = new Vector2D();
        Side = new Vector2D();
        SteeringBehaviour = new SteeringBehaviour(this);
        currentSteeringForce = new Vector2D();
        MaxHealth = Health = 100;
        CurrentTarget = null;
    }

    // velocity of entity  
    public Vector2D Velocity { get; set; }

    // direction of entity
    public Vector2D Heading { get; set; }

    //Perpendicular direction of entity
    public Vector2D Side { get; set; }

    // Mass of entity
    public float Mass { get; set; }

    //Maximum velocity of entity 
    public float MaxSpeed { get; set; }

    //Maximum force on entity
    public float MaxForce { get; set; }

    //Hitpoints
    public float MaxHealth { get; set; }

    //Current Health
    public float Health { get; set; }

    public BaseGameEntity? CurrentTarget { get; set; }

    //Maximum turn rate 
    public float MaxTurnRate { get; set; }

    public SteeringBehaviour SteeringBehaviour { get; set; }

    public override void Update(float timeElapsed) {
        // calculate steering force
        var steeringForce = SteeringBehaviour.Calculate();
        currentSteeringForce = steeringForce.Clone();

        // acceleration force/mass 
        var acceleration = steeringForce.Clone().Divide(Mass);

        // update velocity
        Velocity.Add(acceleration.Multiply(timeElapsed));
        //Velocity.Add(steeringForce);

        // don't exceed max velocity 
        Velocity.Truncate(MaxSpeed);

        // update position 
        Pos.Add(Velocity.Clone().Multiply(timeElapsed));

        // update heading and side if moving
        if (Velocity.LengthSquared() > 0.000000001) {
            Heading = Velocity.Clone().Normalize();
            Side = Heading.Clone().Perpendicular();
        }
    }

    public void CheckCollisions(List<BaseGameEntity> entities) {
        foreach (var entity in entities) {
            if (entity is ItemSpawn item && item.Pos.Clone().Sub(Pos).Length() < item.Radius + Radius)
                if (this is not Projectile) {
                    // heals
                    World.Items.Remove(item);
                    if (Health + 10 >= MaxHealth)
                        Health = MaxHealth;
                    else
                        Health += 10;
                }

            if (entity is Projectile projectile &&
                projectile.Pos.Clone().Sub(Pos.Clone()).Length() < projectile.Radius + Radius)
                if (this is Enemy) {
                    World.MovingEntities.Remove(projectile);
                    if (Health - 10 <= 0)
                        Health = 0;
                    else
                        Health -= 10;
                }
        }
    }

    public override void Render(Graphics g) {
        // RenderDebugInfo(g);
        RenderHp(g);
    }

    //Debug purposes, use if there are issues
    protected void RenderDebugInfo(Graphics g) {
        var drawFont = new Font("Arial", 10);
        var drawBrush = new SolidBrush(Color.Black);
        var x = Pos.X;
        var y = Pos.Y;
        var drawFormat = new StringFormat();

        g.DrawString($"Velocity: {Velocity.ToStringRounded()}", drawFont, drawBrush, x, y, drawFormat);
        g.DrawString($"Heading: {Heading.ToStringRounded()}", drawFont, drawBrush, x, y + 20, drawFormat);
        g.DrawString($"Seek forces: {SteeringBehaviour.CurrentSeek.ToStringRounded()}", drawFont,
            drawBrush, x, y + 40,
            drawFormat);
        g.DrawString($"Arrive forces: {SteeringBehaviour.CurrentArrive.ToStringRounded()}", drawFont,
            drawBrush, x, y + 60,
            drawFormat);
        g.DrawString($"Added forces: {currentSteeringForce.ToStringRounded()}", drawFont, drawBrush, x, y + 80,
            drawFormat);
        g.DrawString($"Obstacle forces: {SteeringBehaviour.CurrentObstacleAvoidance.ToStringRounded()}",
            drawFont,
            drawBrush, x, y + 100,
            drawFormat);

        g.DrawString(SteeringBehaviour.IsCollision ? "Collision: Yes" : "Collision: No", drawFont, drawBrush, x,
            y + 120,
            drawFormat);
    }

    protected void RenderHp(Graphics g) {
        // Draw the health bar
        var healthBarWidth = (int)Radius * 2;
        var healthBarHeight = 4;
        var healthBarX = (int)Pos.X - healthBarWidth / 2;
        var healthBarY = (int)Pos.Y - (int)Radius - healthBarHeight;
        var healthBarMaxWidth = healthBarWidth;

        // Calculate the width of the health bar based on the object's health
        var healthPercent = Health / MaxHealth;
        var healthBarCurrentWidth = (int)(healthPercent * healthBarMaxWidth);

        // Background of the health bar
        g.FillRectangle(Brushes.Gray, healthBarX, healthBarY, healthBarMaxWidth, healthBarHeight);
        // Current health
        g.FillRectangle(Brushes.Green, healthBarX, healthBarY, healthBarCurrentWidth, healthBarHeight);
        // Border of the health bar
        g.DrawRectangle(Pens.Black, healthBarX, healthBarY, healthBarMaxWidth, healthBarHeight);

        // Show the current health value as text on top of the health bar
        var font = new Font("Arial", 10);
        var healthText = string.Format("{0}/{1}", Health, MaxHealth);
        var textSize = g.MeasureString(healthText, font);
        var textPos = new PointF(healthBarX + healthBarWidth / 2 - textSize.Width / 2,
            healthBarY - textSize.Height);
        g.DrawString(healthText, font, Brushes.Black, textPos);
    }

    protected void RenderForceArrows(Graphics g) {
        var scale = 5.0f;
        // velocity of entity
        DrawLineFromEntity(g, Pos, Velocity, Color.Yellow, 3, scale);
        if (SteeringBehaviour.Seek) DrawLineFromEntity(g, Pos, SteeringBehaviour.CurrentSeek, Color.Blue, 3, scale);
        if (SteeringBehaviour.ObstacleAvoidance)
            DrawLineFromEntity(g, Pos, SteeringBehaviour.CurrentObstacleAvoidance, Color.Red, 3, scale);
        if (SteeringBehaviour.Wander)
            DrawLineFromEntity(g, Pos, SteeringBehaviour.CurrentWander, Color.Green, 3, scale);
        if (SteeringBehaviour.Flee)
            DrawLineFromEntity(g, Pos, SteeringBehaviour.CurrentFlee, Color.Orange, 3, scale);
    }

    protected void DrawLineFromEntity(Graphics g, Vector2D start, Vector2D end, Color color, int width, float scale) {
        var scaledEnd = end.Clone().Multiply(scale);
        var endpoint = start.Clone().Add(scaledEnd);

        // Draw the line representing the velocity vector
        g.DrawLine(new Pen(color, width), start.X, start.Y, endpoint.X, endpoint.Y);
    }
}