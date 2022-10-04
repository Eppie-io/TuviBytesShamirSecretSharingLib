# TuviBytesShamirSecretSharingLib
This library realizes Shamir's Secret Sharing Scheme.

It allows you to "divide" your main secret S into N shares. Any T (0 < T <= N <= 16) of them will recover your main secret. You can choose T and N.
Your secret can be a byte or an array of bytes.

How to use:

1. If your secret is a byte the result of sharing will be an array of bytes. 

`byte secret = 199; // any number from 0 to 255`

`byte T = 3;`

`byte N = 5`

`byte[] shares = SecretSharing.SplitSecret(T, N, secret);`

#Important!# You MUST remember indexes of each share to recover main secret. 

To remember index and value of your partitional secret you can use our class Point:

`public class Point
{
    public Field X { get; set; } // index
    public Field Y { get; set; } // value

    public Point (byte x, byte y) // ctor
    {
        X = new Field(x);
        Y = new Field(y);
    }
}`

Class Field is a value in a finite field GF(256). To read more about finite fields: https://en.wikipedia.org/wiki/Finite_field

To recover main secret you should have an array of at least T secret shares (partitional secrets) with its index numbers. 
Each of them should be represented as class Point or Tuple (byte indexNumber, byte Value).

`byte secret = SecretSharing.RecoverSecret(Point[] secretShares)` or

`byte secret = SecretSharing.RecoverSecret((byte, byte)[] secretShares)`

2. If your secret is an array of bytes the result of sharing will be an array of Share (our special class). 
`public class Share
{
    public byte IndexNumber { get; }
    public byte[] ShareValue { get; }

    public Share(byte index, byte[] value) // ctor
}`

`Share[] result = SecretSharing.SplitSecret(4, 6, secret); // T = 4, N = 6`

To recover main secret you should have an array of at least T secret shares (partitional secrets):

byte[] secret = SecretSharing.RecoverSecret(Share[] shares);`



