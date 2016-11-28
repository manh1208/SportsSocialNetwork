package com.capstone.sportssocialnetwork.enumerable;

/**
 * Created by ManhNV on 11/22/16.
 */

public enum NotificationTypeEnum {
    POST (1){
        @Override
        public String toString() {
            return "Sửa chữa";
        }
    },

    ORDER(2){
        @Override
        public String toString() {
            return "Đã được đặt";
        }
    },
    INVITATION(3){
        @Override
        public String toString() {
            return "Sự kiện";
        }
    },
    OTHER(4){
        @Override
        public String toString() {
            return "Khác";
        }
    }


    ;
    private  int value;

    NotificationTypeEnum(int value){
        this.value = value;
    }

    public int getValue() {
        return this.value;
    }

    public static NotificationTypeEnum fromInteger(int i){
        switch (i){
            case 1:
                return POST;
            case 2:
                return ORDER;
            case 3:
                return INVITATION;
            case 4:
                return OTHER;
            default:
                return ORDER;
        }

    }
}
