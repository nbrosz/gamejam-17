using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class BillboardAnimator : MonoBehaviour {

    public new Renderer renderer;
    public Texture[] frames;
    public string[] animations;
    public string startAnimation = null;

    private Dictionary<string, int[]> animationSheets;
    private string currentAnimation = null;

    private float frameLength;
    private int frameCounter = 0;
    private float frameTimer = 0;

    private Queue<string> animationQueue;

    void Awake() {
        animationQueue = new Queue<string>();
        animationSheets = new Dictionary<string, int[]>();
        // convert all animation strings to lists of frames with an identifying name
        foreach(var animationText in animations) {
            var animationName = Regex.Match(animationText, @"[A-z]+").Value;
            var animationFrameString = Regex.Match(animationText, @"(\d+,?)+").Value;
            var animFrameStrings = animationFrameString.Split(',');
            var frameList = new List<int>();

            foreach(var frameString in animFrameStrings) {
                frameList.Add(int.Parse(frameString));
            }

            animationSheets.Add(animationName, frameList.ToArray());
        }

        if (startAnimation != null && startAnimation != "") {
            PlayAnimation(startAnimation); // play starting animation, if specified
        }
    }

    public void PlayAnimation(string animationName, bool clearQueue = false) {
        if (clearQueue)
            animationQueue.Clear();

        currentAnimation = animationName;
        var frameArray = animationSheets[animationName];
        // last entry holds FPS
        frameLength = 1f / frameArray[frameArray.Length-1];
        frameTimer = 0;
        frameCounter = 0;
        SetAnimationFrame(frameArray[frameCounter]);
    }

    public void QueueAnimation(string animationName) {
        animationQueue.Enqueue(animationName);
    }

	void Update () {
		if (currentAnimation != null) {
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameLength) {
                frameTimer = 0;
                var frameArray = animationSheets[currentAnimation];
                frameCounter = (frameCounter + 1) % (frameArray.Length - 1); // -1 because last value is the FPS
                SetAnimationFrame(frameArray[frameCounter]);

                if (frameCounter == 0 && animationQueue.Count > 0) {
                    // play and remove next in queue
                    PlayAnimation(animationQueue.Dequeue());
                }
            }
        }
	}

    void SetAnimationFrame(int index) {
        renderer.material.mainTexture = frames[index];
    }
}
