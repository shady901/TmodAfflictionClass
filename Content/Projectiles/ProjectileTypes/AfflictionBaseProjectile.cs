using AfflictionClass.Content.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AfflictionClass.Content.Projectiles.ProjectileTypes
{
    public abstract class AfflictionBaseProjectile : ModProjectile
    {
        public virtual DamageTypeEnum DamageType { get; set; } = DamageTypeEnum.Generic;
    }
}
