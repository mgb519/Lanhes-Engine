using System.Collections;
using System.Collections.Generic;
using System;

public class RTLTranslation
{

    //TODO: this won't handle cultures with comma fractional seperators, or numbers with thousand seperators
    public static string RTLIfy(string input) {
        string output = "";
        string buf = "";
        foreach (char c in input) {
            if (buf.Length > 0) {
                if ("01234567890".Contains(c.ToString())) {
                    //continuing number, but decimal point must be handled seperately...
                    buf += c;
                } else if (c == '.') {
                    if (buf.Contains(".")) {
                        //this is a full stop! number has ended
                        output += Reverse(buf);
                        buf = "";
                        output += c;
                    } else {
                        //decimal point
                        buf += c;
                    }
                } else {
                    //the number has ended, append it reversed...
                    output += Reverse(buf);
                    buf = "";
                    output += c;
                }
            } else {
                if ("01234567890".Contains(c.ToString())) {
                    //starting a number
                    buf += c;
                } else {
                    output += c;
                }
            }
        }

        output += Reverse(buf);
        return output;


    }

    ///THIS DOES NOT WORK FOR UNICODE!!! WE ONLY USE IT TO INVERT NUMBERS
    private static string Reverse(string s) {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}

