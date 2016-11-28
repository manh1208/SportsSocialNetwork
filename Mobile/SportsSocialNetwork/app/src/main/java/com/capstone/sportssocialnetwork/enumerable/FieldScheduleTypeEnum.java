package com.capstone.sportssocialnetwork.enumerable;

/**
 * Created by ManhNV on 11/5/16.
 */

public enum  FieldScheduleTypeEnum {
    Repair (1){
        @Override
        public String toString() {
            return "Sửa chữa";
        }
    },

    Booked(2){
        @Override
        public String toString() {
            return "Đã được đặt";
        }
    },
    Event(3){
        @Override
        public String toString() {
            return "Sự kiện";
        }
    },
    Other(4){
        @Override
        public String toString() {
            return "Khác";
        }
    }

    ;
    private  int value;

    FieldScheduleTypeEnum(int value){
        this.value = value;
    }

    public int getValue() {
        return this.value;
    }

    public static FieldScheduleTypeEnum fromInteger(int i){
        switch (i){
            case 1:
                return Repair;
            case 2:
                return Booked;
            case 3:
                return Event;
            case 4:
                return Other;

        }
        return null;
    }
}
