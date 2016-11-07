package com.capstone.sportssocialnetwork.enumerable;

/**
 * Created by ManhNV on 10/19/16.
 */
public enum  PaidTypeEnum {

    ChosePayOnline (1){
        @Override
        public String toString() {
            return "Online\nChưa thanh toán";
        }
    },

    ChosePayByCash(2){
        @Override
        public String toString() {
            return "Tiền mặt\nChưa thanh toán";
        }
    },
    PaidByCash(3){
        @Override
        public String toString() {
            return "Tiền mặt\nĐã thanh toán";
        }
    },
    PaidOnline(4){
        @Override
        public String toString() {
            return "Online\nĐã thanh toán";
        }
    }

    ;
    private int value;

    private PaidTypeEnum(int value){
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    public static PaidTypeEnum fromInteger(int i){
        switch (i){
            case 1:
                return ChosePayOnline;
            case 2:
                return ChosePayByCash;
            case 3:
                return PaidByCash;
            case 4:
                return PaidOnline;
        }
        return null;
    }
}
