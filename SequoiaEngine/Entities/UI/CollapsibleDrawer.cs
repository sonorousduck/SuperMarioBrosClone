using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class CollapsibleDrawer
    {
        public Button button;
        public Texture2D BackgroundTexture;
        public Vector2 Position;
        public Vector2 Size;
        public float Rotation;
        public Color SpriteColor = Color.White;
        public AnchorLocation AnchorLocation;
        public ScaleSize Scale;
        public Action<GameObject> OnButtonPress;


        public Texture2D ButtonClosedTexture;
        public Texture2D ButtonOpenTexture;

        public bool IsDrawerOpen;

        public Vector2 DestinationLocationAnimated;
        public Vector2 ClosedPosition;
        public string Tag { get; private set; }

        public GameObject GameObject { get; private set; }
        public GameObject Parent { get; private set; }

        public CollapsibleDrawer(Vector2 position,
            float rotation,
            Vector2 size,
            Texture2D backgroundTexture,
            AnchorLocation anchorLocation = AnchorLocation.None,
            ScaleSize scale = ScaleSize.None,
            string openedButtonFilepath = "", 
            string closedButtonFilepath = "",
            GameObject parent = null,
            string tag = ""
            )
        {
           
            GameObject = new(new Transform(position, rotation, size, true));
            this.Position = position;
            this.BackgroundTexture = backgroundTexture;
            this.Size = size;
            this.AnchorLocation = anchorLocation;
            this.Scale = scale;
            this.Rotation = rotation;
            this.Parent = parent;
            this.Tag = tag;
            this.IsDrawerOpen = true;
            this.DestinationLocationAnimated = new Vector2();

            Setup();

            OnButtonPress = (GameObject) =>
            {
                if (!IsDrawerOpen)
                {
                    Open();
                }
                else
                {
                    Close();
                }
            };



            ButtonClosedTexture = ResourceManager.Get<Texture2D>("closeDrawer");
            ButtonOpenTexture = ResourceManager.Get<Texture2D>("openDrawer");


            button = new Button(new Vector2(10.5f, 33.5f), 0f, Vector2.One, backgroundTexture: ButtonClosedTexture, onPress: OnButtonPress, anchorLocation: AnchorLocation.TopRight, parent: GameObject, tag: "DrawerButton");
        }


        public void Setup()
        {
            float spriteDrawLocationModification = 1.0f;

            GameObject parent = this.GameObject.GetParent();

            if (parent != null && parent.TryGetComponent(out Sprite parentBackground))
            {
                spriteDrawLocationModification = parentBackground.renderDepth - 0.001f;
            }

            GameObject.Add(new Sprite(this.BackgroundTexture, this.SpriteColor, spriteDrawLocationModification));

            if (!this.AnchorLocation.Equals(AnchorLocation.None))
            {
                Anchor anchor = new(this.AnchorLocation);
                GameObject.Add(anchor);


                GameObject.GetComponent<Transform>().position += anchor.GetAnchorPoint(GameObject);
            }

            if (!this.Scale.Equals(ScaleSize.None))
            {
                Scale scale = new(this.Scale);
                GameObject.Add(scale);

                GameObject.GetComponent<Transform>().scale *= scale.GetScaleModifier(GameObject);
            }

            Dictionary<string, Component> test = new Dictionary<string, Component>()
            {
                {"transform", GameObject.GetComponent<Transform>()}
            };

            Action onStart = () =>
            {
                if (IsDrawerOpen)
                {
                    DestinationLocationAnimated = ClosedPosition;
                }
                else
                {
                    DestinationLocationAnimated = this.Position;
                }

                Animated animated = GameObject.GetComponent<Animated>();
                
            };


            Action<float> updatedAnimatedComponent = (float deltaTime) =>
            {
                if (IsDrawerOpen)
                {
                    Transform transform = GameObject.GetComponent<Transform>();
                    Animated animated = GameObject.GetComponent<Animated>();
                    Vector2 test = (this.Position - this.DestinationLocationAnimated);
                    float scale = Math.Clamp(((animated.AnimationTime + deltaTime) / animated.MaxAnimationTime), 0, 1);

                    transform.position = scale * (this.DestinationLocationAnimated) + (1 - scale) * Position;
                }
                else
                {
                    Transform transform = GameObject.GetComponent<Transform>();
                    Animated animated = GameObject.GetComponent<Animated>();
                    float scale = Math.Clamp(((animated.AnimationTime + deltaTime) / animated.MaxAnimationTime), 0, 1);

                    transform.position = scale * (this.DestinationLocationAnimated) + (1 - scale) * ClosedPosition;
                }
                button.AdjustLocation();
            };

            Action onFinished = () =>
            {
                this.IsDrawerOpen = !this.IsDrawerOpen;
            };


            Animated animated = new Animated(0.25f, onStart: onStart, onUpdate: updatedAnimatedComponent, onFinished, extraData: test);
            GameObject.Add(animated); 

            ClosedPosition = new Vector2(-GameObject.GetComponent<Sprite>().size.X / 2f + 10f, this.Position.Y);

        }

        public void AddSubcomponentsToSystemManager(SystemManager systemManager)
        {
            systemManager.Add(button.GameObject);
        }

        public void Close()
        {
            //GameObject.GetComponent<Transform>().scale = new Vector2(0f, 0f);
            //GameObject.GetComponent<Transform>().position = new Vector2(0f, 0f);
            GameObject.GetComponent<Animated>().OnStart?.Invoke();
            button.GameObject.GetComponent<Sprite>().sprite = ButtonOpenTexture;
            //button.AdjustLocation();

        }

        public void Open()
        {
            //GameObject.GetComponent<Transform>().scale = new Vector2(1f, 1f);
            //GameObject.GetComponent<Transform>().position = this.Position;
            GameObject.GetComponent<Animated>().OnStart?.Invoke();
            button.GameObject.GetComponent<Sprite>().sprite = ButtonClosedTexture;

            //button.AdjustLocation();    
        }


    }
}
