using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace Tests
{
    #region Test Module
    public class TestSuite
    {
        private GameManager gameManager;
        private Player player;

        [SetUp]
        public void Setup()
        {
            // Load Resources first
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Game");
            GameObject clone = Object.Instantiate(prefab);
            gameManager = clone.GetComponent<GameManager>();
            player = Object.FindObjectOfType<Player>();
        }

        #region Unit tests
        [UnityTest]
        public IEnumerator GameManagerWasLoaded()
        {
            yield return new WaitForEndOfFrame();

            // Check if resources exist after frame
            Assert.IsTrue(gameManager != null);
        }

        [UnityTest]
        public IEnumerator PlayerExistsInGame()
        {
            yield return new WaitForEndOfFrame();

            Player player = gameManager.GetComponentInChildren<Player>();
            Assert.IsTrue(player != null);
        }

        [UnityTest]
        public IEnumerator ItemCollidesWithPlayer()
        {
            // Get the player
            Player player = Object.FindObjectOfType<Player>();
            // Get an item
            Item item = Object.FindObjectOfType<Item>();

            if (item == null)
                Assert.Fail("Item doesn't exist!");

            // Position both in the same location
            player.transform.position = new Vector3(0, 2, 0);
            item.transform.position = new Vector3(0, 2, 0);

            yield return new WaitForSeconds(0.1f);

            // Asset that item should be destroyed (non-existent)
            Assert.IsTrue(item == null);
        }

        [UnityTest]
        public IEnumerator PlayerShootsItem()
        {
            Player player = Object.FindObjectOfType<Player>();
            Item item = Object.FindObjectOfType<Item>();

            player.transform.position = new Vector3(0, 3, -3);
            item.transform.position = new Vector3(0, 3, 0);

            yield return null;

            Assert.IsTrue(player.Shoot());
        }



        [UnityTest]
        public IEnumerator ScoreIncreasesOnItemCollect()
        {
            // Get an item
            Item item = Object.FindObjectOfType<Item>();

            int oldScore = gameManager.score;

            // Move player on top of item
            player.transform.position = new Vector3(0, 4, 0);
            item.transform.position = new Vector3(0, 4, 0);

            // Wait a few seconds (or until end of Frame)
            yield return new WaitForSeconds(0.1f);

            // GameManager should have 1 score added
            Assert.IsTrue(gameManager.score == oldScore + 1);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(gameManager.gameObject);
        }
        #endregion
    }

    #endregion
}