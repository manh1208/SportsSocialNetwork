package com.capstone.sportssocialnetwork.enumerable;

/**
 * Created by ManhNV on 10/14/16.
 */
public enum RoleEnum {


    MEMBER(1),PLACEOWNER(2),ADMIN(3),MODERATOR(4) ;
    private int value;

    RoleEnum(int value){
        this.value = value;
    }

    public int getValue() {
        return value;
    }
}
