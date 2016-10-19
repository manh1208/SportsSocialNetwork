package com.capstone.sportssocialnetwork.Enumerable;

/**
 * Created by ManhNV on 10/19/16.
 */
public enum OrderStatusEnum {


    Pending (1){
        @Override
        public String toString() {
            return "Đang chờ duyệt";
        }
    },

    Approved(2){
        @Override
        public String toString() {
            return "Đã được chấp nhận";
        }
    },
    Unapproved(3){
        @Override
        public String toString() {
            return "Không được chấp nhận";
        }
    },
    Cancel(4){
        @Override
        public String toString() {
            return "Đã hủy";
        }
    },
    CheckedIn(5){
        @Override
        public String toString() {
            return "Đã nhận sân";
        }
    },
        ;
    private  int value;

    OrderStatusEnum(int value){
        this.value = value;
    }

    public int getValue() {
        return this.value;
    }

    public static OrderStatusEnum fromInteger(int i){
        switch (i){
            case 1:
                return Pending;
            case 2:
                return Approved;
            case 3:
                return Unapproved;
            case 4:
                return Cancel;
            case 5:
                return CheckedIn;
        }
        return null;
    }
}
