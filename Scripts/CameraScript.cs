using Microsoft.Xna.Framework;
using MonoGame.Extended;
using SequoiaEngine;

namespace MarioClone
{
    public class CameraScript : Script
    {
        private Transform transform;
        private MouseInput mouse;
        private Rigidbody rb;
        private GameObject followObject;

        float minX;
        float maxX;
        float minY;
        float maxY;

        private Vector2 currentDirection = new Vector2();

        private float cameraSpeed = 600;


        public CameraScript(GameObject gameObject) : base(gameObject)
        {
        }

        public void SetFollow(GameObject objectToFollow)
        {
            this.followObject = objectToFollow;
        }

        public override void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            transform = gameObject.GetComponent<Transform>();
            mouse = gameObject.GetComponent<MouseInput>();
            minX = 0;
            minY = 0;
            maxX = 640;
            maxY = 360;
        }

        public override void Update(GameTime gameTime)
        {
            if (followObject == null) return;
            transform.position = new Vector2(followObject.GetComponent<Transform>().position.X - 150, followObject.GetComponent<Transform>().position.Y - 225);

            GameManager.Instance.Camera.Position = transform.position;
        }
    }
}