using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleTiles{

    public static TileRule[] waterRules = {
        new TileRule(TILE_TYPE.GRASS).addRule(
            1, 1, 1,
            1, 1, 1,
            1, 1, 1
        ),
        new TileRule(TILE_TYPE.WATER_L).addRule(
            1, 1,-1,
            1, 1, 0,
            1, 1,-1
        ).addRule(
            1, 1, 0,
            1, 1, 1,
            1, 1, 0
        ),
        new TileRule(TILE_TYPE.WATER_R).addRule(
           -1, 1, 1,
            0, 1, 1,
           -1, 1, 1
        ).addRule(
            0, 1, 1,
            1, 1, 1,
            0, 1, 1
        ),
        new TileRule(TILE_TYPE.WATER_T).addRule(
           -1, 0,-1,
            1, 1, 1,
            1, 1, 1
        ).addRule(
            0, 1, 0,
            1, 1, 1,
            1, 1, 1
        ),
        new TileRule(TILE_TYPE.WATER_B).addRule(
            1, 1, 1,
            1, 1, 1,
           -1, 0,-1
        ).addRule(
            1, 1, 1,
            1, 1, 1,
            0, 1, 0
        ),
        new TileRule(TILE_TYPE.WATER_TL).addRule(
            1, 1, 0,
            1, 1, 1,
            1, 1, 1
        ),
        new TileRule(TILE_TYPE.WATER_TR).addRule(
            0, 1, 1,
            1, 1, 1,
            1, 1, 1
        ),
        new TileRule(TILE_TYPE.WATER_BL).addRule(
            1, 1, 1,
            1, 1, 1,
            1, 1, 0
        ),
        new TileRule(TILE_TYPE.WATER_BR).addRule(
            1, 1, 1,
            1, 1, 1,
            0, 1, 1
        ),
        new TileRule(TILE_TYPE.WATER_C_TL).addRule(
           -1,-1,-1,
            1, 1,-1,
            1, 1,-1
        ),
        new TileRule(TILE_TYPE.WATER_C_TR).addRule(
           -1,-1,-1,
           -1, 1, 1,
           -1, 1, 1
        ),
        new TileRule(TILE_TYPE.WATER_C_BL).addRule(
            1, 1,-1,
            1, 1,-1,
           -1,-1,-1
        ),
        new TileRule(TILE_TYPE.WATER_C_BR).addRule(
           -1, 1, 1,
           -1, 1, 1,
           -1,-1,-1
        ),
    };

    

    public static TILE_TYPE GetTile(short[] values) {

        foreach (TileRule rule in waterRules) {
            if (rule.isMatchingRule(values)) {
                return rule.type;
            }
        }

        return TILE_TYPE.WATER;
    }
}

public class TileRule{
    public List<short[]> rules = new List<short[]>();
    public TILE_TYPE type;

    public TileRule(TILE_TYPE type) {
        this.type = type;
    }

    public TileRule addRule(short a, short b, short c, short d, short e, short f, short g, short h, short i) {
        short[] arr = { a, b, c, d, e, f, g, h, i };
        rules.Add(arr);
        return this;
    }

    public bool isMatchingRule(short[] values) {
        foreach (short[] rule in rules) {
            if (equalsRule(rule, values)) return true;
        }
        return false;
    }

    private bool equalsRule(short[] rule, short[] values) {
        for (int i = 0; i < rule.Length; i++) {
            if (rule[i] != -1 && rule[i] != values[i]) {
                return false;
            }
        }
        return true;
    }
}
