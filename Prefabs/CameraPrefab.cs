using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using SequoiaEngine;
using System;
using System.Diagnostics;

namespace MarioClone
{
    public static class CameraPrefab
    {
        public static GameObject Create()
        {
            GameObject camera = new GameObject(new Transform(Vector2.Zero, 0, Vector2.One));

/*            float movementAmount = 200f;


            float maxCameraX = 1000f;
            float minCameraX = 10f;
            float maxCameraY = 250f;
            float minCameraY = -250f;

*/

            

            MouseInput mouseInput = new MouseInput();

            camera.Add(new CameraScript(camera));
            camera.Add(mouseInput);



            return camera;
        }
    }
}