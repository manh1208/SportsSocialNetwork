package com.capstone.sportssocialnetwork.utils;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.media.ExifInterface;
import android.net.Uri;
import android.os.Environment;
import android.provider.MediaStore;
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
import java.util.Locale;
import java.util.TimeZone;
import java.util.concurrent.TimeUnit;

public class Utilities {


    public static Date getDateTime(String s, String format) throws ParseException {
        SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
        Date date = sdf.parse(s);
        return date;
    }

    public static String getDateTimeString(Long s, String format) throws ParseException {
        java.sql.Date date = new java.sql.Date(s);
        SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
        return sdf.format(date);
    }
    public static String getDateTimeString(Time s, String format) throws ParseException {
        SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
        return sdf.format(s);
    }
    public static String getDateTimeString(Date s, String format) throws ParseException {
        SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
        return sdf.format(s);
    }

//    public static Time getTime(String format) throws ParseException{
//        SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
//        Time time = sdf.pa
//    }

    public static Date getZeroTimeDate(Date fecha) {
        Date res = fecha;
        Calendar calendar = Calendar.getInstance();

        calendar.setTime( fecha );
        calendar.set(Calendar.HOUR_OF_DAY, 0);
        calendar.set(Calendar.MINUTE, 0);
        calendar.set(Calendar.SECOND, 0);
        calendar.set(Calendar.MILLISECOND, 0);

        res = calendar.getTime();

        return res;
    }

    public static int exifToDegrees(int exifOrientation) {
        if (exifOrientation == ExifInterface.ORIENTATION_ROTATE_90) { return 90; }
        else if (exifOrientation == ExifInterface.ORIENTATION_ROTATE_180) {  return 180; }
        else if (exifOrientation == ExifInterface.ORIENTATION_ROTATE_270) {  return 270; }
        return 0;
    }

    public static int getOrientation(Context context, Uri photoUri) {

        Cursor cursor = context.getContentResolver().query(photoUri,
                new String[] { MediaStore.Images.ImageColumns.ORIENTATION }, null, null, null);

        if (cursor == null || cursor.getCount() != 1) {
            return 90;  //Assuming it was taken portrait
        }

        cursor.moveToFirst();
        return cursor.getInt(0);
    }


    public static String getTimeAgo(String s) throws ParseException {
        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy HH:mm:ss",Locale.US);
//        Calendar cal = Calendar.getInstance();
//        TimeZone tz = cal.getTimeZone();
//        sdf.setTimeZone(TimeZone.getTimeZone("GMT"));
        Date date = new Date();
        if (s.contains("/Date")){
            s = s.replaceAll("\\D+","");
            date = new Date(Long.parseLong(s));
        }else {
            date = sdf.parse(s);
        }

        Calendar c = Calendar.getInstance();
        Date currentDate = c.getTime();
        long time = 0;
        time = currentDate.getTime() - date.getTime();
        String interval = "";
        long temp = 0;
        if ((temp = TimeUnit.SECONDS.convert(time, TimeUnit.MILLISECONDS)) < 60) {
            if (temp < 10) {
                interval = "Just now";
            } else {
                interval = temp + " second" + (temp > 1 ? "s ago" : " ago");
            }
        } else if ((temp = TimeUnit.MINUTES.convert(time, TimeUnit.MILLISECONDS)) < 60) {
            interval = temp + " minute" + (temp > 1 ? "s ago" : " ago");
        } else if ((temp = TimeUnit.HOURS.convert(time, TimeUnit.MILLISECONDS)) < 24) {
            interval += temp + " hour" + (temp > 1 ? "s ago" : " ago");
        } else if ((temp = TimeUnit.DAYS.convert(time, TimeUnit.MILLISECONDS)) < 30) {
            if (temp==1){
                interval = "Yesterday";
            }else {
                interval += temp + " day" + (temp > 1 ? "s ago" : " ago");
            }
        } else {
            interval = s;
        }
        return interval;
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
            bitmap = scaleDown(bitmap, maxPicel, false);
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
