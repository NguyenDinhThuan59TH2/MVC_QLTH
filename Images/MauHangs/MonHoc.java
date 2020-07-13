/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package templatemethod_bai1;

/**
 *
 * @author Admin
 */
public class MonHoc {
    private int maMH;
    private String tenMH;
    private int soTC;

    public MonHoc(int maMH, String tenMH, int soTC) {
        this.maMH = maMH;
        this.tenMH = tenMH;
        this.soTC = soTC;
    }

    public int getMaMH() {
        return maMH;
    }

    public String getTenMH() {
        return tenMH;
    }

    public int getSoTC() {
        return soTC;
    }
    
    
}
