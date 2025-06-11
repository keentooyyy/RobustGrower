using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This helper script allows for cleaner and safer access to animations by name.
/// It loads all animation clips from the Animator’s controller at runtime and stores them in a dictionary.
/// This enables triggering animations by name using a simple method call.
/// Attach this script to any GameObject with an Animator component.
/// </summary>
public class AnimatorControllerHelper : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    private Dictionary<string, string> animationStates; // Dictionary to store animation names


    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError("Animator not assigned!");
            return;
        }

        // Initialize the animationStates dictionary here in Awake()
        animationStates = new Dictionary<string, string>();

        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        if (controller != null)
        {
            foreach (AnimationClip clip in controller.animationClips)
            {
                if (!animationStates.ContainsKey(clip.name))
                {
                    animationStates.Add(clip.name, clip.name);
                }
            }

            // Optional: Log loaded animation names
            // Debug.Log("Loaded Animations: " + string.Join(", ", animationStates.Keys));
        }
        else
        {
            Debug.LogError("Animator has no RuntimeAnimatorController assigned.");
        }
    }

    /// <summary>
    /// Plays the specified animation by name if it exists in the dictionary.
    /// </summary>
    /// <param name="animationName">The name of the animation to play.</param>
    public void PlayAnimation(string animationName)
    {
        if (animationStates == null)
        {
            Debug.LogError("Animation states dictionary is not initialized.");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is null.");
            return;
        }

        if (animationStates.ContainsKey(animationName))
        {
            animator.Play(animationStates[animationName]);
        }
        else
        {
            Debug.LogWarning("Animation not found: " + animationName);
        }
    }
}
