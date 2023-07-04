﻿using BepInEx.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using File = System.IO.File;
using HarmonyLib;

namespace KingdomMod
{
    public class DevTools : MonoBehaviour
    {
        private static ManualLogSource log;
        private bool enableDebugInfo = false;
        private readonly GUIStyle guiStyle = new();
        private int tick = 0;
        private readonly List<DebugInfo> debugInfoList = new();

        public static void Initialize(DevToolsPlugin plugin)
        {
            log = plugin.Log;
            var component = plugin.AddComponent<DevTools>();
            component.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(component.gameObject);
        }

        public DevTools()
        {
            Patcher.PatchAll();
            log.LogMessage($"isDebugBuild: {Debug.isDebugBuild}");
        }

        private void Start()
        {
            log.LogMessage($"{this.GetType().Name} Start.");

            CoinBag.OverflowLimit = 1000;
        }

        public class Patcher
        {
            public static void PatchAll()
            {
                try
                {
                    var harmony = new Harmony("KingdomMod.BetterPayableUpgrade.Patcher");
                    harmony.PatchAll();
                }
                catch (Exception ex)
                {
                    log.LogError($"[Patcher] => {ex}");
                }
            }

            [HarmonyPatch(typeof(UnityEngine.Debug), "get_isDebugBuild")]
            public class DebugIsDebugBuildPatcher
            {
                public static void Postfix(ref bool __result)
                {
                    __result = true;
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.End))
            {
                log.LogMessage("End key pressed.");
                enableDebugInfo = !enableDebugInfo;
            }

            if (Input.GetKeyDown(KeyCode.Insert))
            {
                log.LogMessage("Insert key pressed.");

                var cursorSystem = GameObject.FindObjectOfType<CursorSystem>();
                if (cursorSystem)
                    cursorSystem.SetForceVisibleCursor(true);

                var biomeHelper = GameObject.Find("BiomeHelper");
                if (biomeHelper)
                {
                    var biomeHolder = biomeHelper.GetComponent<BiomeHolder>();
                    if (biomeHolder)
                    {
                        biomeHolder.debugToolsEnabled = true;
                    }
                }

                InterfaceOverlay.DebugDisable = false;

                // DebugTools.Inst.OpenButtonPressed();
                //
                // var debugTools = GameObject.Find("DebugTools");
                // if (debugTools)
                // {
                //     var iDebugTools = new IDebugTools(debugTools.Pointer);
                //     iDebugTools.OpenButtonPressed();
                //
                //     // debugTools.SetState(DebugTools.State.Open);
                //     // var component = debugTools.GetComponent<DebugTools>();
                //     // if (component)
                //     // {
                //     //     component.SetState(DebugTools.State.Open);
                //     //     Plugin.Logger.LogInfo("DebugTools SetState Open");
                //     // }
                // }

            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                List<GameObjectDetails> objectTree = new List<GameObjectDetails>();

                log.LogMessage("Dumping All Objects to JSON using GetAllScenesGameObject()...");
                foreach (var obj in GameObjectDetails.GetAllScenesGameObjects())
                {
                    objectTree.Add(new GameObjectDetails(obj));
                }

                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\AllObjectsDump.json",
                    GameObjectDetails.JsonSerialize(objectTree));

                log.LogMessage("Complete!");
                log.LogMessage("JSON written to " +
                               (AppDomain.CurrentDomain.BaseDirectory + "\\AllObjectsDump.json").Replace("\\\\", "\\"));

                string path = (AppDomain.CurrentDomain.BaseDirectory + "\\AllObjectsDump.json").Replace("\\\\", "\\");
                Application.OpenURL("file:///" + path.Replace("\\", "/"));
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                log.LogMessage($"Dump Prefabs:");

                var prefabList = GameObject.FindObjectsOfType<PrefabID>();
                foreach (var prefab in prefabList)
                {
                    log.LogMessage($"PrefabID:{prefab.prefabID}, name: {prefab.name}");
                }

                var prefabs = Managers.Inst.prefabs;
                if (prefabs != null)
                {
                    var dumpStr = "\npublic enum PrefabIDs\n{\n";
                    foreach (var prefab in prefabs.prefabMasterCopies)
                    {
                        var prefabName = prefab.name.Replace(' ', '_');
                        dumpStr = dumpStr + "    " + prefabName + " = " + prefab.prefabID + ",\n";
                    }

                    dumpStr = dumpStr + "}\n";
                    log.LogMessage(dumpStr);
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                log.LogMessage($"Dump LevelBlocks:");

                var levelBlocks = Managers.Inst.level.GetLevelBlocks();
                log.LogMessage($"LevelBlocks: {levelBlocks.Count}");
                foreach (var levelBlock in levelBlocks)
                {
                    log.LogMessage($"groups: {levelBlock.groupOne}, {levelBlock.groupTwo}, {levelBlock.groupThree}");
                }
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                var player = Managers.Inst.kingdom.playerOne;
                if (player != null)
                {
                    var payable = player.selectedPayable;
                    if (payable != null)
                    {
                        log.LogMessage($"Try to destroy the game object: {payable.GetGO.name}");
                        Destroy(payable.GetGO);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                var kingdom = Managers.Inst.kingdom;
                if (kingdom != null)
                {
                    log.LogMessage($"Try to add Peasant.");

                    var prefab = Resources.Load<Peasant>("Prefabs/Characters/Peasant").gameObject;
                    Vector3 vector = kingdom.playerOne.transform.TransformPoint(new Vector3(1f, 1f, 0.01f));
                    var lastSpawned = Pool.SpawnOrInstantiateGO(prefab, vector, Quaternion.identity, null);
                    lastSpawned.transform.SetParent(GameObject.FindGameObjectWithTag("GameLayer").transform);
                }
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                var kingdom = Managers.Inst.kingdom;
                if (kingdom != null)
                {
                    log.LogMessage($"Try to add Griffin.");

                    var prefab = Resources.Load<Steed>("Prefabs/Steeds/Griffin P1");
                    prefab.price = 1;
                    Vector3 vector = kingdom.playerOne.transform.TransformPoint(new Vector3(1f, 1f, 0.01f));
                    var lastSpawned = Pool.SpawnOrInstantiateGO(prefab.gameObject, vector, Quaternion.identity, null);
                    lastSpawned.transform.SetParent(GameObject.FindGameObjectWithTag("GameLayer").transform);
                }
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                var kingdom = Managers.Inst.kingdom;
                if (kingdom != null)
                {
                    log.LogMessage($"Try to add Wall0.");

                    // var prefab = Resources.Load<Wall>("prefabs/buildings and interactive/wall0");
                    Vector3 vector = new Vector3(kingdom.playerOne.transform.position.x, 0.0f, 0.1f);

                    Wall wall = Instantiate<Wall>(Managers.Inst.holder.wallPrefabs[0]);
                    wall.transform.SetParent(GameObject.FindGameObjectWithTag("GameLayer").transform);
                    wall.transform.position = vector;

                    // var lastSpawned = Pool.SpawnOrInstantiateGO(prefab.gameObject, vector, Quaternion.identity, GameObject.FindGameObjectWithTag("GameLayer").transform);
                    // lastSpawned.transform.SetParent(GameObject.FindGameObjectWithTag("GameLayer").transform);
                }
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                var kingdom = Managers.Inst.kingdom;
                if (kingdom != null)
                {
                    log.LogMessage($"Try to add Tower0.");

                    // var prefab = Resources.Load<Tower>("prefabs/buildings and interactive/tower0");

                    var prefab = Managers.Inst.prefabs.GetPrefabById((int)PrefabIDs.Tower0);
                    prefab.GetComponent<PayableUpgrade>().nextPrefab = Managers.Inst.prefabs.GetPrefabById((int)PrefabIDs.Tower2);

                    Vector3 vector = new Vector3(kingdom.playerOne.transform.position.x, 0.0f, 1.6f);
                    var lastSpawned = Pool.SpawnOrInstantiateGO(prefab.gameObject, vector, Quaternion.identity,
                        GameObject.FindGameObjectWithTag("GameLayer").transform);
                    // lastSpawned.transform.SetParent(GameObject.FindGameObjectWithTag("GameLayer").transform);
                }
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                Transform[] array = Resources.FindObjectsOfTypeAll<Transform>();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].name == "DebugTools")
                    {
                        array[i].gameObject.SetActive(true);
                        UnityEngine.UI.Text[] componentsInChildren = array[i].gameObject.GetComponentsInChildren<UnityEngine.UI.Text>(true);
                        for (int j = 0; j < componentsInChildren.Length; j++)
                        {
                            componentsInChildren[j].font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                            componentsInChildren[j].fontSize = 4;
                            componentsInChildren[j].fontStyle = FontStyle.Bold;
                            componentsInChildren[j].text = componentsInChildren[j].text.ToLower();
                        }

                        var iDebugTools = new IDebugTools(array[i].GetComponent("DebugTools").Pointer);

                        var cursorSystem = GameObject.FindObjectOfType<CursorSystem>();
                        if (cursorSystem)
                            cursorSystem.SetForceVisibleCursor(!iDebugTools.IsOpen());

                        iDebugTools.OpenButtonPressed();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                var player = Managers.Inst.kingdom.playerOne;
                player.TeleportFlash();
                player.coinLaunchSound.Play(player.transform.position, 1);

                GameObject gameObject = Pool.SpawnGO(Resources.Load<Boulder>("Prefabs/Objects/Boulder").gameObject, Vector3.zero, Quaternion.identity);
                var compCacher = Managers.Inst.compCacher;
                compCacher.GetCachedComponent<Boulder>(gameObject)._launchedByEnemies = false;
                compCacher.GetCachedComponent<Boulder>(gameObject).maxHitCitizens = 1000;
                compCacher.GetCachedComponent<Boulder>(gameObject).StuckProbability = 0f;
                compCacher.GetCachedComponent<Boulder>(gameObject).hitDamage = 300;
                compCacher.GetCachedComponent<SpriteRendererFX>(gameObject).FadeIn(0.5f);
                gameObject.transform.parent = player.transform.parent;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.identity;
                compCacher.GetCachedComponent<Boulder>(gameObject).SetFake();
                gameObject.transform.SetParent(player.transform.parent, true);
                gameObject.transform.localScale = new Vector3(3f, 3f, 3f);
                gameObject.transform.position = player.transform.position;
                gameObject.transform.rotation = Quaternion.LookRotation(player.transform.forward);

                Vector2 vector6 = new Vector2((int)player.mover.GetDirection(), 0.2f);
                Vector2 vector7 = vector6.normalized * 14f;
                compCacher.GetCachedComponent<Boulder>(gameObject).Launch(vector7, player.gameObject);
                Pool.Despawn(gameObject, 5f);
            }

            tick = tick + 1;
            if (tick > 60)
                tick = 0;

            if (tick == 0)
            {
                if (!IsPlaying()) return;

                if (enableDebugInfo)
                    UpdateDebugInfo();

            }
        }

        private void OnGUI()
        {
            if (!IsPlaying()) return;

            if (enableDebugInfo)
                DrawDebugInfo();
        }

        private bool IsPlaying()
        {
            if (!Managers.Inst) return false;
            if (!Managers.Inst.game) return false;
            return Managers.Inst.game.state is Game.State.Playing or Game.State.Menu;
        }

        private void UpdateDebugInfo()
        {
            var worldCam = Managers.Inst.game._mainCameraComponent;
            if (!worldCam) return;

            debugInfoList.Clear();
            var objects = GameObject.FindObjectsOfType<GameObject>();
            foreach (var obj in objects)
            {
                var renderer = obj.GetComponent<Renderer>();
                if (!renderer || renderer.isVisible == false) continue;
                if (obj.GetComponent<Bird>()) continue;
                if (obj.GetComponent<Cloud>()) continue;
                if (obj.GetComponent<Grass>()) continue;
                if (obj.GetComponent<Flame>()) continue;
                if (obj.GetComponent<Tile>()) continue;
                if (obj.GetComponent<Foliage>()) continue;
                if (obj.GetComponent<ParticleSystem>()) continue;
                if (obj.GetComponent<BackgroundWall>()) continue;
                if (obj.GetComponent<ReflectedLight>()) continue;
                if (obj.GetComponent<Monument>()) continue;
                if (obj.GetComponent<Fish>()) continue;
                if (obj.GetComponent<Parallax2DChild>()) continue;
                // if (obj.GetComponent<Rigidbody2D>()) continue;
                // if (obj.GetComponent<PolygonCollider2D>()) continue;
                if (obj.GetComponent<WorkableTree>()) continue;

                if (obj.name.StartsWith("Haze")) continue;
                if (obj.name.StartsWith("castle_back")) continue;

                if (obj.name == "Glow") continue;
                if (obj.name == "Puff") continue;
                if (obj.name == "trunk") continue;
                if (obj.name == "sparkles") continue;
                if (obj.name == "highlight") continue;
                if (obj.name == "Roots(Clone)") continue;

                Vector3 screenPos = worldCam.WorldToScreenPoint(obj.transform.position);
                Vector3 uiPos = new Vector3(screenPos.x, Screen.height - screenPos.y, 0);
                if (uiPos.x > 0 && uiPos.x < Screen.width && uiPos.y > 0 && uiPos.y < Screen.height)
                {
                    debugInfoList.Add(new DebugInfo(new Rect(uiPos.x, uiPos.y, 100, 100), obj.transform.position, obj.name));
                    //  + " (" + obj.transform.position.ToString() + ")(" + uiPos.ToString() + ")"
                    log.LogInfo(obj.name);
                }
            }
        }

        private void DrawDebugInfo()
        {
            guiStyle.normal.textColor = Color.white;
            foreach (var obj in debugInfoList)
            {
                GUI.Label(obj.Pos, obj.Info, guiStyle);
                // var vecPos = obj.pos;
                // vecPos.y += 20;
                // GUI.Label(vecPos, obj.vec.x.ToString(), SpotMarkGUIStyle);
            }

            // var dog = GameObject.FindObjectOfType<Dog>();
            // if (!dog)
            //     return;
            //
            // Vector3 dogScreenPos = worldCam.WorldToScreenPoint(dog.transform.position);
            // Vector3 dogUiPos = new Vector3(dogScreenPos.x, Screen.height - dogScreenPos.y, 0);
            // GUI.Label(new Rect(dogUiPos.x, dogUiPos.y, 100, 100), dog.name, SpotMarkGUIStyle);

        }

        public class DebugInfo
        {
            public Rect Pos;
            public Vector3 Vec;
            public string Info;

            public DebugInfo(Rect pos, Vector3 vec, string info)
            {
                this.Pos = pos;
                this.Vec = vec;
                this.Info = info;
            }
        }

        public enum PrefabIDs
        {
            None = -1,
            Castle0 = 0,
            Castle1 = 1,
            Castle2 = 2,
            Castle3 = 3,
            Castle4 = 4,
            Castle5 = 5,
            Castle6 = 6,
            Farmhouse0 = 7,
            Farmhouse1 = 8,
            Farmhouse2 = 9,
            Tower0 = 10,
            Tower1 = 11,
            Tower2 = 12,
            Tower3 = 13,
            Tower4 = 14,
            Wall0 = 15,
            Wall1 = 16,
            Wall2 = 17,
            Wall3 = 18,
            Wall4 = 19,
            Wreck = 20,
            Quarry_undeveloped = 21,
            Quarry = 22,
            Tree = 23,
            Chest = 24,
            Wall1_Wreck = 25,
            Wall2_Wreck = 26,
            Wall3_Wreck = 27,
            Wall4_Wreck = 28,
            Wall5_Wreck = 29,
            Wall4_horn = 30,
            Wall5_horn = 31,
            Wall5 = 32,
            Tower_Baker = 33,
            Tower_Ballista = 34,
            Tower_Knight = 35,
            Lighthouse_undeveloped = 36,
            Beach = 37,
            Wharf = 38,
            Beggar_Camp = 39,
            Portal = 40,
            Teleporter = 41,
            TeleporterRift = 42,
            Cliff_Portal = 43,
            BoatSailPosition = 44,
            Lighthouse_Stone = 45,
            Lighthouse_Iron = 46,
            Lighthouse_Wood = 47,
            Castle7 = 48,
            Mine_undeveloped = 49,
            Mine = 50,
            oakTree = 51,
            Forge = 52,
            ShopPike = 53,
            Workshop = 54,
            ShopScythe = 55,
            CaveSpawnerTree = 56,
            Title = 57,
            Tower5 = 58,
            Tower6 = 59,
            MerchantHouse = 60,
            FarmhouseStable = 61,
            BeachPortal = 62,
            BoatSailPosition_Stone = 63,
            Citizen_House = 64
        }

    }
}