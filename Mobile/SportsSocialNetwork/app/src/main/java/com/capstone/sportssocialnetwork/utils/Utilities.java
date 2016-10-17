package com.capstone.sportssocialnetwork.utils;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Environment;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TimePicker;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.sql.Time;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

public class Utilities {


    public static Date getDateTime(String s, String format) throws ParseException {
        SimpleDateFormat sdf = new SimpleDateFormat(format);
        Date date = sdf.parse(s);
        return date;
    }

    public static String getDateTimeString(Long s, String format) throws ParseException {
        java.sql.Date date = new java.sql.Date(s);
        SimpleDateFormat sdf = new SimpleDateFormat(format);
        return sdf.format(date);
    }
    public static String getDateTimeString(Time s, String format) throws ParseException {
        SimpleDateFormat sdf = new SimpleDateFormat(format);
        return sdf.format(s);
    }
    public static String getDateTimeString(Date s, String format) throws ParseException {
        SimpleDateFormat sdf = new SimpleDateFormat(format);
        return sdf.format(s);
    }

    public static String getPicturePath(String name) {
        String dir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES) + "/" + name;
        return dir;
    }

    public static File getImageFileFromUri(Context context, Uri imageUri, String imagePath, int maxPicel, Bitmap.CompressFormat imageType, int imageQuality) {

        File f = new File(imagePath);
        try {
            f.createNewFile();
            InputStream imageStream = context.getContentResolver().openInputStream(imageUri);
            Bitmap bitmap = BitmapFactory.decodeStream(imageStream);
            bitmap = scaleDown(bitmap, 2000, false);
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            bitmap.compress(imageType, imageQuality /*ignored for PNG*/, bos);
            byte[] bitmapdata = bos.toByteArray();
            FileOutputStream fos = new FileOutputStream(f);
            fos.write(bitmapdata);
            fos.flush();
            fos.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return f;
    }

    public static Bitmap scaleDown(Bitmap realImage, float maxImageSize,
                                   boolean filter) {
        float ratio = Math.min(
                (float) maxImageSize / realImage.getWidth(),
                (float) maxImageSize / realImage.getHeight());
        int width = Math.round((float) ratio * realImage.getWidth());
        int height = Math.round((float) ratio * realImage.getHeight());

        Bitmap newBitmap = Bitmap.createScaledBitmap(realImage, width,
                height, filter);
        return newBitmap;
    }

    public static void setDateField(Context mContext,EditText editText, String format) {
        final String finalFormat = format;
        final Context finalContext = mContext;
        final  EditText finalEdittext  = editText;
        Calendar newCalendar = Calendar.getInstance();
        final DatePickerDialog datePickerDialog = new DatePickerDialog(mContext, new DatePickerDialog.OnDateSetListener() {
            public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
                Calendar newDate = Calendar.getInstance();
                newDate.set(year, monthOfYear, dayOfMonth);

                try {
                    finalEdittext.setText(getDateTimeString(newDate.getTime(),finalFormat));
                } catch (ParseException e) {
                    e.printStackTrace();
                }
            }

        }, (newCalendar.get(Calendar.YEAR)), (newCalendar.get(Calendar.MONTH)), (newCalendar.get(Calendar.DATE)));
        editText.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (hasFocus)

                    datePickerDialog.show();
                InputMethodManager imm = (InputMethodManager) finalContext.getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.hideSoftInputFromWindow(finalEdittext.getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
            }
        });
        editText.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                datePickerDialog.show();
                InputMethodManager imm = (InputMethodManager) finalContext.getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.hideSoftInputFromWindow(finalEdittext.getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
            }
        });
    }

    public static void setTimeField(Context mContext,EditText editText, String formatTime) {
        final String finalFormattime = formatTime;
        final Context finalContext = mContext;
        final  EditText finalEdittext  = editText;
        final Calendar newCalendar = Calendar.getInstance();
        final TimePickerDialog timePickerDialog = new TimePickerDialog(mContext, new TimePickerDialog.OnTimeSetListener() {
            @Override
            public void onTimeSet(TimePicker view, int hourOfDay, int minute) {
                Time time = new Time(hourOfDay, minute, 0);
                try {
                    finalEdittext.setText(getDateTimeString(time,finalFormattime));
                } catch (ParseException e) {
                    e.printStackTrace();
                }
            }
        }, newCalendar.get(Calendar.HOUR_OF_DAY), newCalendar.get(Calendar.MINUTE), true);
        editText.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (hasFocus)
                    timePickerDialog.show();
                InputMethodManager imm = (InputMethodManager) finalContext.getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.hideSoftInputFromWindow(finalEdittext.getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
            }
        });
        editText.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                timePickerDialog.show();
                InputMethodManager imm = (InputMethodManager) finalContext.getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.hideSoftInputFromWindow(finalEdittext.getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
            }
        });


    }

    public static void setDatetimeField(Context mContext,EditText editText, String formatDate, String formatTime) {
        final String finalFormatdate = formatDate;
        final String finalFormattime = formatTime;
        final Context finalContext = mContext;
        final  EditText finalEdittext  = editText;

        final Calendar newCalendar = Calendar.getInstance();
        final String[] dateTime = {"", ""};
        final TimePickerDialog timePickerDialog = new TimePickerDialog(mContext, new TimePickerDialog.OnTimeSetListener() {
            @Override
            public void onTimeSet(TimePicker view, int hourOfDay, int minute) {
                Time time = new Time(hourOfDay, minute, 0);
                try {
                    dateTime[1] = getDateTimeString(time,finalFormattime);
                } catch (ParseException e) {
                    e.printStackTrace();
                }
                finalEdittext.setText(dateTime[0] + " " + dateTime[1]);
            }
        }, newCalendar.get(Calendar.HOUR_OF_DAY), newCalendar.get(Calendar.MINUTE), true);
        final DatePickerDialog datePickerDialog = new DatePickerDialog(mContext, new DatePickerDialog.OnDateSetListener() {
            public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
                Calendar newDate = Calendar.getInstance();
                newDate.set(year, monthOfYear, dayOfMonth);
                try {
                    dateTime[0] = getDateTimeString(newDate.getTime(),finalFormatdate);
                } catch (ParseException e) {
                    e.printStackTrace();
                }
                timePickerDialog.show();

            }

        }, (newCalendar.get(Calendar.YEAR)), (newCalendar.get(Calendar.MONTH)), (newCalendar.get(Calendar.DATE)));
        editText.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (hasFocus)
                    datePickerDialog.show();
                InputMethodManager imm = (InputMethodManager) finalContext.getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.hideSoftInputFromWindow(finalEdittext.getWindowToken(), 0);
            }
        });
        editText.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                datePickerDialog.show();
                InputMethodManager imm = (InputMethodManager) finalContext.getSystemService(Context.INPUT_METHOD_SERVICE);
                imm.hideSoftInputFromWindow(finalEdittext.getWindowToken(), 0);
            }
        });


    }

}
