using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequoiaEngine
{
    public class TiledMapComponent : Component
    {
        public TiledMap TiledMap;
        public float RenderDepth = 1.0f;

        public TiledMapComponent(TiledMap map, float renderDepth = 1.0f)
        {
            this.TiledMap = map;
            RenderDepth = renderDepth;
        }


        public List<GameObject> GetCollisionLayer(string layerName)
        {
            List<GameObject> results = new List<GameObject>();
            TiledMapObjectLayer tiledMapLayer = this.TiledMap.GetLayer<TiledMapObjectLayer>(layerName);


            if (tiledMapLayer.Objects.Length > 0)
            {
                foreach (TiledMapObject tiledObject in tiledMapLayer.Objects)
                {
                    Vector2 size = new Vector2(tiledObject.Size.Width, tiledObject.Size.Height).ToInt();

                    GameObject go = new(new Transform(tiledObject.Position.ToInt() + size / 2, 0, Vector2.One));
                    RectangleCollider rectangleCollider = new(size, true, CollisionLayer.Ground, CollisionLayer.Player);
                    go.Add(rectangleCollider);
                    go.Add(new Rigidbody());
                    go.Add(new RenderedComponent());

                    results.Add(go);
                }

                return results;
            }
            else
            {
                Debug.WriteLine($"Collision layer of layer name {layerName} does not exist!");
                return null;
            }

        }

        public List<GameObject> GetInteractables(string layerName, Dictionary<string, Func<Vector2, Vector2, TiledMapProperties, GameObject>> classNameToGameObject)
        {
            List<GameObject> results = new();
            TiledMapObjectLayer tiledMapLayer = this.TiledMap.GetLayer<TiledMapObjectLayer>(layerName);

            if (tiledMapLayer.Objects.Length > 0)
            {
                foreach (TiledMapTileObject tiledObject in tiledMapLayer.Objects)
                {
                    Vector2 size = new Vector2(tiledObject.Size.Width, tiledObject.Size.Height).ToInt();
                    string className = tiledObject.Tile.Type.ToString();

                    if (classNameToGameObject.ContainsKey(className))
                    {
                        GameObject go = classNameToGameObject[className](tiledObject.Position, size, tiledObject.Properties);
                        results.Add(go);
                    }
                }
            }

            return results;
        }
    }
}
