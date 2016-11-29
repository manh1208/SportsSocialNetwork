package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

import java.util.List;

/**
 * Created by ManhNV on 10/12/16.
 */
public class User {
    @SerializedName("Id")
    private String Id;
    @SerializedName("FullName")
    private String fullName;
    @SerializedName("AvatarImage")
    private String avatar;
    @SerializedName("AspNetRoles")
    private List<Role> roles;
    @SerializedName("Email")
    private String email;
    @SerializedName("PhoneNumber")
    private String PhoneNumber;
    @SerializedName("UserName")
    private String userName;
    @SerializedName("Address")
    private String address;
    @SerializedName("City")
    private String city;
    @SerializedName("District")
    private String district;
    @SerializedName("Ward")
    private String ward;
    @SerializedName("CoverImage")
    private String coverImage;
    @SerializedName("Hobbies")
    private List<Hobby> hobbies;
    @SerializedName("Birthday")
    private String birthDay;
    @SerializedName("BirthdayString")
    private String birthDayString;
    @SerializedName("Gender")
    private String gender;
    @SerializedName("CreateDate")
    private String createDate;
    @SerializedName("Followed")
    private boolean followed;
    @SerializedName("FollowCount")
    private int followCount;
    @SerializedName("FollowedCount")
    private int followedCount;
    @SerializedName("PostCount")
    private int postCount;


    public String getId() {
        return Id;
    }

    public void setId(String id) {
        Id = id;
    }

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }

    public String getAvatar() {
        return avatar;
    }

    public void setAvatar(String avatar) {
        this.avatar = avatar;
    }

    public Role getRole() {
        if (roles != null && roles.size() > 0) {
            return roles.get(0);
        } else {
            return null;
        }
    }

    public List<Role> getRoles() {
        return roles;
    }

    public void setRoles(List<Role> roles) {
        this.roles = roles;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPhoneNumber() {
        return PhoneNumber;
    }

    public void setPhoneNumber(String phoneNumber) {
        PhoneNumber = phoneNumber;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public String getCity() {
        return city;
    }

    public void setCity(String city) {
        this.city = city;
    }

    public String getDistrict() {
        return district;
    }

    public void setDistrict(String district) {
        this.district = district;
    }

    public String getWard() {
        return ward;
    }

    public void setWard(String ward) {
        this.ward = ward;
    }

    public String getCoverImage() {
        return coverImage;
    }

    public void setCoverImage(String coverImage) {
        this.coverImage = coverImage;
    }

    public List<Hobby> getHobbies() {
        return hobbies;
    }

    public void setHobbies(List<Hobby> hobbies) {
        this.hobbies = hobbies;
    }

    public String getBirthDay() {
        return birthDay;
    }

    public void setBirthDay(String birthDay) {
        this.birthDay = birthDay;
    }

    public String getBirthDayString() {
        return birthDayString;
    }

    public void setBirthDayString(String birthDayString) {
        this.birthDayString = birthDayString;
    }

    public String getGender() {
        return gender;
    }

    public void setGender(String gender) {
        this.gender = gender;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public String getAddressString() {
        String tmp = address;
        if (ward != null && ward.length() > 0) {
            tmp += " - " + ward;
        }
        if (district != null && district.length() > 0) {
            tmp += " - " + district;
        }
        if (city != null && city.length() > 0) {
            tmp += " - " + city;
        }
        if (tmp==null || tmp.length()<=0){
            tmp = "Không xác định";
        }

        return tmp;
    }

    public String getListSport() {
        String s = "";
        if (hobbies!=null) {
            for (Hobby hobby : this.hobbies
                    ) {
                s += hobby.getSportName() + ", ";

            }
            if (s.length()>0) {
                s = s.substring(0, s.length() - 2);
            }
        }
        return s;
    }


    public boolean isFollowed() {
        return followed;
    }

    public void setFollowed(boolean followed) {
        this.followed = followed;
    }

    public int getFollowCount() {
        return followCount;
    }

    public void setFollowCount(int followCount) {
        this.followCount = followCount;
    }

    public int getFollowedCount() {
        return followedCount;
    }

    public void setFollowedCount(int followedCount) {
        this.followedCount = followedCount;
    }

    public int getPostCount() {
        return postCount;
    }

    public void setPostCount(int postCount) {
        this.postCount = postCount;
    }
}
