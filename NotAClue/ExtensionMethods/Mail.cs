using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;

namespace NotAClue
{
	public static class Email
	{
		/// <summary>
		/// Adds the addresses.
		/// </summary>
		/// <param name="theAddressCollection">The address collection.</param>
		/// <param name="theAddresses">The addresses.</param>
		public static void AddAddresses(this MailAddressCollection theAddressCollection, MailAddressCollection theAddresses)
		{
			foreach (var address in theAddresses)
			{
				theAddressCollection.Add(address);
			}
		}

		/// <summary>
		/// Adds the addresses.
		/// </summary>
		/// <param name="theAddressCollection">The address collection.</param>
		/// <param name="theAddresses">The addresses.</param>
		public static void AddAddresses(this MailAddressCollection theAddressCollection, IEnumerable<MailAddress> theAddresses)
		{
			foreach (var address in theAddresses)
			{
				theAddressCollection.Add(address);
			}
		}

		/// <summary>
		/// To the mail addresses.
		/// </summary>
		/// <param name="addresses">The addresses.</param>
		/// <returns>MailAddressCollection.</returns>
		public static MailAddressCollection ToMailAddressCollection(this String addresses)
		{
			var mailAddresses = new MailAddressCollection();
			var addressList = addresses.ToList();
			foreach (var address in addressList)
			{
				mailAddresses.Add(address.ToMailAddress());
			}
			return mailAddresses;
		}

		/// <summary>
		/// To the mail address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns>MailAddress.</returns>
		public static MailAddress ToMailAddress(this String address)
		{
			var addressParts = address.ToList(separator: '|');
			var mailAddress = new MailAddress(addressParts[0], addressParts[1]);

			return mailAddress;
		}
	}
}
