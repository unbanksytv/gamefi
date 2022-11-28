using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;

public class Web3 : MonoBehaviour
{
    private ThirdwebSDK sdk;

    public GameObject disconnectedState;

    public GameObject connectedState;

    public GameObject loadingState;

    public GameObject loadedStateNoNfts;

    public GameObject loadedStateYesNfts;

    public GameObject kartNftOne;

    public GameObject kartNftTwo;

    public GameObject playButton;

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
        // If in Unity preview:
        if (!Application.isEditor)
        {
            string address = await sdk.wallet.Connect();
            int chain = await sdk.wallet.GetChainId();
            if (chain != 420)
            {
                sdk.wallet.SwitchNetwork(420);
            }
        }

        disconnectedState.SetActive(false);
        connectedState.SetActive(true);

        loadingState.SetActive(true);
        var nfts = await LoadNFTs();
        loadingState.SetActive(false);

        if (nfts.Count > 0)
        {
            loadedStateYesNfts.SetActive(true);
            DisplayOwnedNFTs (nfts);
        }
        else
        {
            loadedStateNoNfts.SetActive(true);
        }
    }

    public async void ClaimNFT(string tokenId)
    {
        await GetKartNFTCollection().ERC1155.Claim(tokenId, 1);
        loadedStateNoNfts.SetActive(false);
        loadedStateYesNfts.SetActive(true);
        DisplayOwnedNFTs(await LoadNFTs());
    }

    public async Task<List<NFT>> LoadNFTs()
    {
        // Get user wallet address
        string address = await sdk.wallet.GetAddress();

        // Get NFTs owned by user
        return await GetKartNFTCollection().ERC1155.GetOwned(address);
    }

    public void DisplayOwnedNFTs(List<NFT> nfts)
    {
        bool ownsTokenZero = nfts.Exists(nft => nft.metadata.id == "0");
        bool ownsTokenOne = nfts.Exists(nft => nft.metadata.id == "1");

        if (ownsTokenOne)
        {
            // If Owns token 1, show "Roadster_Player" GameObject
            kartNftTwo.SetActive(true);
        }
        else
        {
            // If Owns token 0, show "KartClassic_Player" GameObject
            kartNftOne.SetActive(true);
        }
    }
}
