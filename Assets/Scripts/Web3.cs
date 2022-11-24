using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;

public class Web3 : MonoBehaviour
{
    private ThirdwebSDK sdk;

    // Start is called before the first frame update
    void Start()
    {
        sdk = new ThirdwebSDK("optimism-goerli");
    }

    // Update is called once per frame
    void Update()
    {
    }

    Contract GetKartNFTCollection()
    {
        return sdk.GetContract("0x1Cd921cC9B802929a161193b2D614f962881968B"); // NFT Drop
    }

    public async void ConnectWallet()
    {
        string address = await sdk.wallet.Connect();
        int chain = await sdk.wallet.GetChainId();
        if (chain != 420)
        {
            sdk.wallet.SwitchNetwork(420);
        }

        GameObject.Find("DisconnectedState").SetActive(false);
        GameObject.Find("ConnectedState").SetActive(true);

        var nfts = await LoadNFTs();
        GameObject.Find("LoadingState").SetActive(true);

        if (nfts.Count > 0)
        {
            GameObject.Find("LoadedStateOwnsNFT").SetActive(true);
        }
        else
        {
            GameObject.Find("LoadedStateNoNFT").SetActive(true);
        }
    }

    public async void ClaimNFT(string tokenId)
    {
        await GetKartNFTCollection().ERC1155.Claim(tokenId, 1);

        // Disable "LoadedStateNoNFT" and enable "LoadedStateOwnsNFT"
        GameObject.Find("LoadedStateNoNFT").SetActive(false);
        GameObject.Find("LoadedStateOwnsNFT").SetActive(true);
    }

    public async Task<List<NFT>> LoadNFTs()
    {
        // Get user wallet address
        string address = await sdk.wallet.GetAddress();

        // Get NFTs owned by user
        return await GetKartNFTCollection().ERC1155.GetOwned(address);
    }

    public async void DisplayOwnedNFTs(List<NFT> nfts)
    {
        // Display NFTs on the UI
        // If Owns token 0, show "KartClassic_Player" GameObject
        GameObject
            .Find("KartClassic_Player")
            .SetActive(nfts.Exists(nft => nft.metadata.id == "0"));

        // If Owns token 1, show "Roadster_Player" GameObject
        GameObject
            .Find("Roadster_Player")
            .SetActive(nfts.Exists(nft => nft.metadata.id == "1"));
    }
}
