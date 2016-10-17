using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class BoardSetup {
    private static Random rng = new Random();

    private List<HexType> remainingTiles;
    private List<int> remainingTokens;

    private int tileIndex = -1;
    private int tokenIndex = -1;

    public BoardSetup() {
        remainingTiles = new List<HexType>();
        remainingTokens = new List<int>();

        addMultiple(HexType.FIELD, remainingTiles, 4);
        addMultiple(HexType.FOREST, remainingTiles, 4);
        addMultiple(HexType.PASTURE, remainingTiles, 4);

        addMultiple(HexType.HILLS, remainingTiles, 4);
        addMultiple(HexType.MOUNTAIN, remainingTiles, 4);

        addMultiple(HexType.DESERT, remainingTiles, 1);
        shuffle(remainingTiles);
        shuffle(remainingTiles);

        for (int i = 2; i <= 12; i++) {
            if(i == 2 || i == 12) remainingTokens.Add(i);
            else if(i != 7) addMultiple(i, remainingTokens, 2);
        }
        shuffle(remainingTokens);
        shuffle(remainingTokens);
    }

    private void addMultiple<T>(T toAdd, List<T> list, int quantity) {
        for(int i = 0; i < quantity; i++) list.Add(toAdd);
    }

    private void shuffle<T>(List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int randomIndex = rng.Next(list.Count);
            T temp = list[randomIndex];
            list[randomIndex] = list[i];
            list[i] = temp;

        }
    }

    public HexType getRandomHex() {
        tileIndex++;
        return remainingTiles[tileIndex];
    }

    public int getRandomToken() {
        tokenIndex++;
        return remainingTokens[tokenIndex];
    }
}
