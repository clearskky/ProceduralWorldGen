using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStation : MonoBehaviour, IInteractable, IBuilding
{
    public int interactableId { get; set; }

    public static float[] miningSpeedLevelModifiers         = new float[]   {0, 1.8f, 3, 4, 6, 8.5f, 11};
    public static int[]   miningSpeedLevelUpgradePrices     = new int[]     {0, 750, 2000, 5000, 20000, 100000, 500000};

    public static int[]   storageLevelModifiers             = new int[]     {0, 5, 15, 30, 60, 110, 170};
    public static int[]   storageLevelUpgradePrices         = new int[]     {0, 750, 2000, 5000, 20000, 100000, 500000};

    public static float[] fuelTankLevelModifiers            = new float[]   {0, 15, 30, 50, 70, 90, 110};
    public static int[]   fuelTankLevelUpgradePrices        = new int[]     {0, 750, 2000, 5000, 20000, 100000, 500000};

    public static float[] radarLevelModifiers = new float[] { 0.6f, 0.5f, 0.4f, 0.3f, 0.25f, 0.20f, 0.15f };
    public static int[] radarLevelUpgradePrices = new int[] { 0, 250, 500, 1000, 2000, 10000, 20000 };

    public int maxMiningSpeedUpgradeCount;
    public int maxStorageUpgradeCount;
    public int maxFuelTankUpgradeCount;
    public int maxRadarUpgradeCount;

    public Button upgradeMiningSpeedLevelButton;
    public Button upgradeStorageLevelButton;
    public Button upgradeFuelTankLevelButton;
    public Button upgradeRadarLevelButton;

    public Button returnToGameButton;

    public Text currentMiningSpeedLevelText;
    public Text currentStorageLevelText;
    public Text currentFuelTankLevelText;
    public Text currentRadarLevelText;

    //public Text miningSpeedLabel;
    //public Text storageLabel;
    //public Text fuelTankLabel;

    public Text miningSpeedLevelUpgradePriceText;
    public Text storageLevelUpgradePriceText;
    public Text fuelTankLevelUpgradePriceText;
    public Text radarLevelUpgradePriceText;

    public Player interactingPlayer;

    void Start()
    {
        interactableId = gameObject.GetInstanceID();
    }

    public void OnTriggerEnter2D(Collider2D actor)
    {
        interactingPlayer = actor.GetComponent<Player>();
        interactingPlayer.OnInteract += OnInteract;
    }

    public void OnTriggerExit2D(Collider2D actor)
    {
        interactingPlayer = actor.GetComponent<Player>();
        interactingPlayer.OnInteract -= OnInteract;
    }

    public void OnInteract(object sender, OnInteractEventArgs playerData)
    {
        interactingPlayer.fuelCanDrain = false;
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.UpgradeVendor);

        maxMiningSpeedUpgradeCount = miningSpeedLevelModifiers.Length;
        maxStorageUpgradeCount = storageLevelModifiers.Length;
        maxFuelTankUpgradeCount = fuelTankLevelModifiers.Length;

        interactingPlayer = playerData.interactingPlayer;

        AdjustUIElements();
    }

    private void AdjustUIElements()
    {
        // Mining speed
        if (interactingPlayer.miningSpeedLevel < maxMiningSpeedUpgradeCount)
        {
            currentMiningSpeedLevelText.text = interactingPlayer.miningSpeedLevel.ToString();
            miningSpeedLevelUpgradePriceText.text = "$" + miningSpeedLevelUpgradePrices[interactingPlayer.miningSpeedLevel + 1].ToString();
        }
        else
        {
            upgradeMiningSpeedLevelButton.enabled = false;
            miningSpeedLevelUpgradePriceText.enabled = false;
        }

        // Storage
        if (interactingPlayer.storageLevel < maxStorageUpgradeCount)
        {
            currentStorageLevelText.text = interactingPlayer.storageLevel.ToString();
            storageLevelUpgradePriceText.text = "$" + storageLevelUpgradePrices[interactingPlayer.storageLevel + 1].ToString();
        }
        else
        {
            upgradeStorageLevelButton.enabled = false;
            storageLevelUpgradePriceText.enabled = false;
        }

        // Fuel Tank
        if (interactingPlayer.fuelTankLevel < maxFuelTankUpgradeCount)
        {
            currentFuelTankLevelText.text = interactingPlayer.fuelTankLevel.ToString();
            fuelTankLevelUpgradePriceText.text = "$" + fuelTankLevelUpgradePrices[interactingPlayer.fuelTankLevel + 1].ToString();
        }
        else
        {
            upgradeFuelTankLevelButton.enabled = false;
            fuelTankLevelUpgradePriceText.enabled = false;
        }

        // Radar
        if (interactingPlayer.vignetteController.radarLevel < maxRadarUpgradeCount)
        {
            currentRadarLevelText.text = interactingPlayer.vignetteController.radarLevel.ToString();
            radarLevelUpgradePriceText.text = "$" + radarLevelUpgradePrices[interactingPlayer.vignetteController.radarLevel + 1].ToString();
        }
        else
        {
            upgradeRadarLevelButton.enabled = false;
            radarLevelUpgradePriceText.enabled = false;
        }
    }

    

    public void UpgradeMiningSpeed()
    {
        if (interactingPlayer.cash >= miningSpeedLevelUpgradePrices[interactingPlayer.miningSpeedLevel + 1])
        {
            interactingPlayer.AdjustCash(miningSpeedLevelUpgradePrices[interactingPlayer.miningSpeedLevel + 1] * -1);
            interactingPlayer.miningSpeedLevel += 1;            
            AdjustUIElements();
            interactingPlayer.AdjustMiningDrillProperties();
        }
    }

    public void UpgradeStorage()
    {
        if (interactingPlayer.cash >= storageLevelUpgradePrices[interactingPlayer.storageLevel + 1])
        {
            interactingPlayer.AdjustCash(miningSpeedLevelUpgradePrices[interactingPlayer.storageLevel + 1] * -1);
            interactingPlayer.storageLevel += 1;            
            AdjustUIElements();
            interactingPlayer.AdjustMiningDrillProperties();
        }
    }

    public void UpgradeFuelTank()
    {
        if (interactingPlayer.cash >= fuelTankLevelUpgradePrices[interactingPlayer.fuelTankLevel + 1])
        {
            interactingPlayer.AdjustCash(miningSpeedLevelUpgradePrices[interactingPlayer.fuelTankLevel + 1] * -1);
            interactingPlayer.fuelTankLevel += 1;            
            AdjustUIElements();
            interactingPlayer.AdjustMiningDrillProperties();
        }
    }

    public void UpgradeRadar()
    {
        if (interactingPlayer.cash >= radarLevelUpgradePrices[interactingPlayer.vignetteController.radarLevel + 1])
        {
            interactingPlayer.AdjustCash(radarLevelUpgradePrices[interactingPlayer.vignetteController.radarLevel + 1] * -1);
            interactingPlayer.vignetteController.radarLevel += 1;            
            AdjustUIElements();
            interactingPlayer.vignetteController.AdjustVignetteProperties();
        }
    }

    public void ReturnToPlayerInterface()
    {
        interactingPlayer.fuelCanDrain = true;
        CanvasManager.Instance.EnableSpecificPanel(TogglablePanelType.PlayerInterface);

    }
}
