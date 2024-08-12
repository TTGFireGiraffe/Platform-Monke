using BepInEx;
using System;
using UnityEngine;
using Utilla;

namespace Air_Jump
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        public static GameObject leftPlatform;

        public static bool leftPlatformEnabled = false;

        public static GameObject rightPlatform;

        public static bool rightPlatformEnabled = false;

        public bool platformLeft;

        public bool platformRight;

        private static float transitionDuration = 3f;

        private static float elapsedTime = 0f;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
        }

        void Update()
        {
            if (inRoom == true)
            {
                if (ControllerInputPoller.instance.leftGrab)
                {
                    if (!leftPlatformEnabled)
                    {
                        leftPlatform = GameObject.CreatePrimitive((PrimitiveType)3);
                        leftPlatform.GetComponent<Renderer>().material.color = Color.black;
                        leftPlatform.GetComponent<Renderer>().material = new Material(Shader.Find("Sprites/Default"));
                        leftPlatform.transform.localScale = new Vector3(0.28f, 0.015f, 0.38f);
                        leftPlatform.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position + new Vector3(0f, -0.02f, 0f);
                        leftPlatform.transform.rotation = GorillaLocomotion.Player.Instance.leftControllerTransform.rotation * Quaternion.Euler(0f, 0f, -90f);
                        leftPlatformEnabled = true;
                    }
                    if (elapsedTime < transitionDuration)
                    {
                        leftPlatform.GetComponent<Renderer>().material.color = Color.black;
                        elapsedTime += Time.deltaTime;
                        if (elapsedTime >= transitionDuration)
                        {
                            elapsedTime = 0f;
                        }
                    }
                }
                else
                {
                    if (leftPlatformEnabled)
                    {
                        UnityEngine.Object.Destroy(leftPlatform);
                        leftPlatformEnabled = false;
                        return;
                    }
                }
                if (ControllerInputPoller.instance.rightGrab)
                {
                    if (!rightPlatformEnabled)
                    {
                        rightPlatform = GameObject.CreatePrimitive((PrimitiveType)3);
                        rightPlatform.GetComponent<Renderer>().material.color = Color.black;
                        rightPlatform.GetComponent<Renderer>().material = new Material(Shader.Find("Sprites/Default"));
                        rightPlatform.transform.localScale = new Vector3(0.28f, 0.015f, 0.38f);
                        rightPlatform.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position + new Vector3(0f, -0.02f, 0f);
                        rightPlatform.transform.rotation = GorillaLocomotion.Player.Instance.rightControllerTransform.rotation * Quaternion.Euler(0f, 0f, -90f);
                        rightPlatformEnabled = true;
                    }
                    if (elapsedTime < transitionDuration)
                    {
                        rightPlatform.GetComponent<Renderer>().material.color = Color.black;
                        elapsedTime += Time.deltaTime;
                        if (elapsedTime >= transitionDuration)
                        {
                            elapsedTime = 0f;
                        }
                    }
                }
                else
                {
                    if (rightPlatformEnabled)
                    {
                        UnityEngine.Object.Destroy(rightPlatform);
                        rightPlatformEnabled = false;
                        return;
                    }
                }
                if (!ControllerInputPoller.instance.leftGrab)
                {
                    UnityEngine.Object.Destroy(leftPlatform);
                }
                if (!ControllerInputPoller.instance.rightGrab)
                {
                    UnityEngine.Object.Destroy(rightPlatform);
                }
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
