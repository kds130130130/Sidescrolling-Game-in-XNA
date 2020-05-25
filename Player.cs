using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeepGrinding
{
    class Player
    {
        //NOTE: Using Public instead of Private for a reason?
        //NOTE: Using int instead of float for a reason?
        private int health;
        private float attack;
        private float defense;
        private float speed;
        private float points;

        public Player()
        {
            points = 100.0f;
            attack = 1f;
            defense = 1f;
            speed = 1f;
            health = 100;
        }

        public void setStats(int h, int a, int s, int d)
        {
            health = h;
            attack = a;
            speed = s;
            defense = d;
        }
        public void resetStats()
        {
            health = 100;
            attack = 1;
            speed = 1;
            defense = 1;
            points = 100;
        }
        public void setHealth(int h)
        {
            health = h;
        }
        public void setSpeed(int s)
        {
            speed = s;
        }
        public void setAttack(int a)
        {
            attack = a;
        }
        public void setDefense(int d)
        {
            defense = d;
        }
        public void takeDamage(float t)
        {
            health -= (int)t;
        }
        public float getSpeed()
        {
            return speed;
        }
        public float getAttack()
        {
            return attack;
        }
        public float getDefense()
        {
            return defense;
        }
        public int getHealth()
        {
            return health;
        }
        public void addSpeed(float s)
        {
            speed += s;
            points -= s;
        }
        public void addAttack(float a)
        {
            attack += a;
            points -= a;
        }
        public void addDefense(float d)
        {
            defense += d;
            points -= d;
        }
        public void setPoints(int desiredPoints) 
        {
            points = desiredPoints;
        }
        public int getPoints()
        {
            return (int)points;
        }
    }
}
