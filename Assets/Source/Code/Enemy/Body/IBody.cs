using System;

namespace RedRockStudio.SZD.Enemy
{
    public interface IBody
    {
        event Action Damaged;

        public IBodyPart Head { get; }

        public IBodyPart Torso { get; }

        public IBodyPart Arms { get; }

        public IBodyPart Legs { get; }
    }
}