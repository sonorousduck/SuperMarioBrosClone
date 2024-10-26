using Microsoft.Xna.Framework;
using SequoiaEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarioClone
{
    public class GameEventsManager : SequoiaEngine.System
    {
        public static GameEventsManager Instance;

        GameEventsManager(SystemManager systemManager) : base(systemManager) 
        {
            Instance = this;
        }

        protected override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
