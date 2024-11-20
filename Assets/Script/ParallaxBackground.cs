using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform[] layerParts; // Parts of the layer (original and duplicate)
        public float speedMultiplier;  // How fast this layer moves relative to others
    }

    public ParallaxLayer[] layers; // Array of parallax layers
    public float baseSpeed = 2f;   // Base scrolling speed of the background

    void Update()
    {
        foreach (var layer in layers)
        {
            // Move each layer
            foreach (Transform part in layer.layerParts)
            {
                part.Translate(Vector2.left * baseSpeed * layer.speedMultiplier * Time.deltaTime);

                // Reset position when a part goes out of view
                if (part.position.x <= -part.GetComponent<SpriteRenderer>().bounds.size.x)
                {
                    Vector3 newPosition = part.position;
                    newPosition.x += part.GetComponent<SpriteRenderer>().bounds.size.x * 2;
                    part.position = newPosition;
                }
            }
        }
    }
}
