using Thirdweb;
using UnityEngine;

public class TokenClaimer : MonoBehaviour
{
    private ThirdwebSDK sdk;

    public GameObject balanceText;

    public GameObject claimButton;

    private Contract getTokenDrop()
    {
        return sdk.GetContract("0x4a9659d5E0d416Ce8B9a4336132012Af8db4c5AB");
    }

    async void OnEnable()
    {
        sdk = new ThirdwebSDK("optimism-goerli");
        await sdk.wallet.Connect();

        // Set text to user's balance
        var bal = await getTokenDrop().ERC20.Balance();

        balanceText.GetComponent<TMPro.TextMeshProUGUI>().text =
            bal.displayValue + " " + bal.symbol;
    }

    public async void Claim()
    {
        await sdk.wallet.Connect();
        await sdk.wallet.SwitchNetwork(420);
        await getTokenDrop().ERC20.Claim("25");

        // hide claim button
        claimButton.SetActive(false);

        // Run OnEnable again to update balance
        OnEnable();
    }
}
