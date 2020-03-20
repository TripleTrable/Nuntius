using RSAEncryption;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace nuntiusClientChat.Controller
{
	public static class UserController
	{

		//TODO: Prop LogedInUser
		//public static nuntiusModel.User LogedInUser = new nuntiusModel.User();
		public static nuntiusModel.User LogedInUser = null;
		private static string currentToken = null;

		/// <summary>
		/// NO
		/// </summary>
		/// <returns></returns>
		public static Encryption GetEncryption()
		{
			if (UserRsaKeys == null)
			{
				Encryption encryption = new RSAEncryption.Encryption();
				encryption.PrivateKey = "<RSAKeyValue><Modulus>r1An5MaxM7+KHWGoZAjBFwyij5HlTHgXY5Qc74dlZtvrh6X1Xsn7wQx9ZxVp21KHrYQz8UUg5uCyTax2cDJ+XWsb1//JhSDaEuoN7iOotJ+bpoPqlc+E5ytj+t76U48AfcjJKrgIm5bp1bAmhiyOot8ZKWLooS0mOeGLg0yuDQmeGQskPkx3/vZZVgcGriJ88MVRK0Bz0WjAuJzlSZTnnWjAyx0EtlgAgAVh2A110DVgsAU6JJYoEjD+bfDP6rFw3pCJ2a8vjMk2hfqNXVjhAMFq/QM9FOGtGNkN+Zi+rA4ri9eZWmRiIVKYdoXFtFU586NyIkxFdHKPPfJaQIXSgw1XkyEepzIcb9KMEkwa2rMKIMHTVsXtJ/MguEIKd9WhZ15nBVVJqBPZoLCQV8vgGcb2s4NvD83GYn6yX8VmiEBoW4f9Wz1ZsuyzzfAc/GxNcYCtqZOAiOxIrY9LbAad5XzBA00Zh6szNaYb4Hp5C1x5U+ZSkRVgNTWQJ0334HeUJvnXvuZRT38j0uaMJstHWvWC4XimLwDDVblY4Qsz+CN5qN2cPrKUAQAdeLDtQAoU3Xh2fFApRgq4X4EF0u8W7ngSaLGW63/gG3DqoAtLkPba/7XXQVx2mq620UbnAHN0aZ+IiC/qrW8y7ey679IXBJ4IvyXJvbr2l3AA1L7fMdc=</Modulus><Exponent>AQAB</Exponent><P>3shB0n7hpGwVnoMPIzGMek9tQGgEl3+9t1y3RCQ5zF4nFczcaAwKsQgCAM4QXTRaYR522OcD2/pEZi+CT5s5p9B/QaWcHxXTaCoQekfi1n3DfeorZMUwd8PzKl6nczKv9yL9uGMMw3NKugftQrr8pmOTEOVn37FxIoHDTKk1NUzmIlF1DNgZh3RCK/KWgp23SoZWKmTzAD+UQEYAwG0chbjJbCzh+xif89/ajZXAYbcDtfaqO9krM9rSxifZWR99aM7uEvYiduuvtKXtGyBBpHcdRNHQXGxlvgSs4FMhpdw7dJ26OHsa0d/zs+4JXrH3ClcBT/irGaqptwKOHolDWQ==</P><Q>yXP4f3pD1ezZq/CdYZMj9A2oiw9OORcp9W64FhYJ6AMOy3J2ziYcV4ls+6P+cX98A4IsTBlr2dUJdPazFE+WE8ohtY3CAdOexisJSd4XC9JtgEdsbVw8aa/yvlOHy7TpkDbDBZ25wgeb46j3lnFJhYBzQjmrH1JCsR02VNtWvcY36Z8Z8aNVAPoGFEC0pI+SCzPGHiGr+1qPgNM7IEqrGXF/4rRN1daz2phGZMjIrrywCW7A5qI78IjXeodHeE3lIQ5b3mvamTnsDVa5S7Upt4pUURDL+G//fRW+ifg7ErA2RXAPXQIkVu8Pf2FxlKPyOMECHK8sVIk/n3GjSV5orw==</Q><DP>CRma/rMqGmJD2860rpZWi6R07P2SunGAWV4TKlhkeAGcjRpqImHjiemu00OXjYcW7gKljiSZlsG5S4dDRmcryrwMhqzyHJ7ynL2jIKuRC3vloV4QWbRoT2wUobVuF5mJcIngXUjGe7FY6xJp0aD7svw8RKNqQ8vmuCceyCHdR4kVmW9EXbryCANqtIrbNQimNQgZuu+WsXrnXly9qR6L3LZ4m8+Vh4Ew/3A0aDYmvpcQMr2ZpIoxZpPYqjl0elx77e0N8n5VWBA2hCWG8uW1aLdo3afIYA4ZddKRETS0GidN0myi762vocUJGl9tyI1ybtyw9AhPTOQYvJ4BGiQ3OQ==</DP><DQ>b0ZHtOJsJfY2Jes6g3MrINuKhYVmx9Irsw39UPXLcwR9X5NSXHxgm1mlgToMidTJQ4bbwQ0praEnIBxEvqtXUocMJ6StzSR/RybucobiY/1PztOk6KNSG268vrSVBchgWvBkuO1udeaODSp/1/Grkrpo7+O6ygo+HGbuY+DkAN9Ecx8WzYpSob95LCaxoVRMbzOh83LV9HkJWzdSKXwVFnDDGw5NEBkQL2Z9O55Me3huJEew9bQXN73eiCfBKoqcTa4IwMsCbDC6WoPUxiTDbkZSXJTSleAHV6hzHvKmbqQ21hQX6GAOgpZ6EPpaKqiEWyUBbaa07d0B76htcVFcsQ==</DQ><InverseQ>qCWEu2x1hkSOkLlZD3sDFLuBZk+cmtjNPC+VKbKZ1f5foZY53J6Ott3cybjJnJr8Av4zYJOz+USJfYtQzIEdfTrVwn98Th8TOfUDiIDkICcShaZGGc5V4l8sBi5YF1k4FF1dVg1VYk5wsk2XrwyLQWMHc/cznHBovLb3UNR9KE2xf9OPrVnSLcurOyfxWLZqtoXKHlOnl65qqvd80hQPAUdlnohgPDt51Kaj7LwZaJKmS7stGgK68wB7dKEGVW6/QYFCt9TqawjPa7DUbmPf+T2UNZmPPzA8S5H4EIOKevS6UwOgIPwQv1H0RqwfumuxNtDa/7vJ3BHnXmNF2KCEKQ==</InverseQ><D>OmCHmD+tfBzY4eXGC+F6YrF7ZDFh2G/Y4fjNFdnRI0oyCM5zgWfi5CrDi1LvF/BIa7gtD7dEOH00AndeTQgTWgozkF6FfUYuN8QmdVj8cYEAdJdSwyYCDHQ/a6daVf61XC1DB22WuDCtltF8Uot9x7BgkY51Bk6hfv6i1UrwXjkIozA5lmEiSA1CPEpEqDGlxwr9d1iT+HRYUGE2XKUv5xOdt0Tnb8yoBflE1ovM4lMT6ikjblFt8HjOEE/y/CSv//zDWnBidg/+juiasC3LZtQBS1ULvutTF4YXiDibrXFegMd/MkFhB9WYHUd4SQ9kJ8QSWmdpbVhEaI1ODlRBf4Lqc00vIR3PBPrMnP+eqp6JvK/CfjLw0KBa/9bpVqKH3LLfSAngasEV/i8LMqTTW3DgVb0MO1UfmojHQMmLSCs40loqWJVir7x0jqxhKUBHy3Zn2AW7JPxaVE0ek8Ww52RFST2bMZ3c3Lxh236fsL33yYgtylup3A+AHwRgtxEUYqPNqv8ZEjs0woGX7zOa1qmNUlQu/mcpi5EtqFMYXZOBVsGRemgazO1OMISswzMmvlz42O6MCkBKjrxo4hDMh2L/30iDl64uvRvD9LAWA3mn3hK4BUgdypS/zI+EaCXx3nNUxylnLSRVWzD5HkaaqQBET05bgnOnFBA/65gm/jE=</D></RSAKeyValue>";
				return UserRsaKeys = encryption;
			}
			else
			{
				return UserRsaKeys;
			}
		}

		public static string CurrentTocken
		{
			get { return currentToken; }
			set { currentToken = value; }
		}

		public static Encryption UserRsaKeys { get; set; }



	}
}
