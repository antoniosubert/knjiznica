/*
 * ----------------------------------------------
 * 
 * Autor: Antonio Šubert
 * Projekt: Evidencija knjiga u knjižnici
 * Predmet: Osnove programiranja
 * Ustanova: VŠMTI
 * Godina: 2019.
 * 
 * ----------------------------------------------
 */
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using ConsoleTables;
using System.Xml.Linq;
using System.Globalization;
using System.Linq;

namespace Knjiznica
{
	class Program
	{
		public struct Prijava 
		{
			public string korisnickoIme;
			public string lozinka;
			public Prijava(string ime, string loz) 
			{
				korisnickoIme = ime;
				lozinka = loz;
			}
		}
		public struct Knjiga 
		{
			public string sifra;
			public string naziv;
			public string autorID;
			public string godinaIzadavanja;
			public Knjiga(string s, string n, string aId, string godIz) 
			{
				sifra = s;
				naziv = n;
				autorID = aId;
				godinaIzadavanja = godIz;
			}
		}
		public struct Autor 
		{
			public string redniBroj;
			public string imeAutora;
			public Autor(string rb, string ia) 
			{
				redniBroj = rb;
				imeAutora = ia;
			}
		}
		public struct KnjigaA
		{
			public string sifra;
			public string naziv;
			public string autor;
			public string godinaIzadavanja;

			public KnjigaA(string s, string n, string a, string godIz)
			{
				sifra = s;
				naziv = n;
				autor = a;
				godinaIzadavanja = godIz;
			}
		}
		static List<Knjiga> Knjige = new List<Knjiga>();
		static List<string> Godine = new List<string>();
		static List<Autor> Autori = new List<Autor>();
		static int brojKnjiga;
		static bool prijavljen = false;
		static bool adminPrij = false;
		static int signal = 0;


		public static void Statistika()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija omogućuje korisniku da ima uvid u
			 * statistiku o određenim kriterijima koji su
			 * zadani u programu i korisnik ih svojom voljom 
			 * odabire.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			string tekst = "Pokrenuta je statistika";
			Loganje(tekst);
			int odabir1;
			Console.WriteLine("\nOdaberite opciju 1 ili 2.");
			Console.WriteLine("\n1-->Ukupan broj knjiga po godini izdanja");
			Console.WriteLine("\n2-->Broj knjiga po autoru");
			Console.WriteLine("\nENTER-->Povratak na glavni izbornik");
			string izbor;
			izbor = Console.ReadLine();
			if (izbor == "")
			{
				Console.Clear();
				TkojePrijavljen();
			}
			odabir1 = Convert.ToInt32(izbor);
			switch (odabir1)
			{
				case 1:
					{
						DohvatiKnjige();
						DohvatiAutore();
						int brKnjiga = 0;
						Godine.Clear();
						Console.Clear();
						foreach (var godina in Knjige)
						{
							if (!Godine.Contains(godina.godinaIzadavanja))
							{
								Godine.Add(godina.godinaIzadavanja);
							}

						}
						foreach (var godina in Godine)
						{
							foreach (var Knjiga in Knjige)
							{
								
								if (Knjiga.godinaIzadavanja == godina)
								{
									foreach (var autor in Autori)
									{
										if (Knjiga.autorID == autor.redniBroj)
										{
											Console.WriteLine("{0} - {1}", Knjiga.naziv, autor.imeAutora);
										}
									}
									brKnjiga++;
								}
							}
							Console.WriteLine("Ima {0}. knjiga koje su izdane {1}. godine", brKnjiga, godina);
							Console.WriteLine("\n");
							brKnjiga = 0;
						}
						Console.ReadKey(); 
						Console.Clear();
						TkojePrijavljen();
						break;
					}
				case 2:
					{
						DohvatiKnjige();
						DohvatiAutore();
						int brAutor = 0;
						Console.Clear();
						foreach (var autor in Autori)
						{

							foreach (var Knjiga in Knjige)
							{
								if (Knjiga.autorID == autor.redniBroj)
								{
									Console.WriteLine("{0} - {1}", Knjiga.naziv, Knjiga.godinaIzadavanja);
									brAutor++;
								}
							}
							Console.WriteLine("Autor: {0} je napisao {1} knjiga", autor.imeAutora, brAutor);
							Console.WriteLine("\n");
							brAutor = 0;
						}
						Console.ReadKey();
						Console.Clear();
						TkojePrijavljen();
						break;
					}
				default:
					{
						break;
					}
			}
		}
		public static void Loganje(string tekst)
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja zapisuje određene akcije u
			 * programu u tekstualnu datoteku.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			string path1 = "logovi.txt";
			StreamWriter Log = new StreamWriter(path1, true);
			Log.Write(DateTime.Now + " -> {0} " + "\n", tekst);
			Log.Flush();
			Log.Close();
		}
		public static void DodavanjeKnjige()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja omogućuje korisniku dodavanje
			 * nove knjige u sustav.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			string tekst = "Pokrenuto je dodavanje knjiga";
			Loganje(tekst);
			DohvatiKnjige();
			DohvatiAutore();
			int odabir1;
			string naslov;
			string odabirAutora = "";
			string sifra;
			string godIzdavanja;
			string AutorB = "";
			string rBroj = "";
			ponovno1:
			Console.WriteLine("\nDODAVANJE KNJIGE\n");
			Console.WriteLine("Unesite naslov knjige: ");
			naslov = Console.ReadLine();
			foreach (var imeKnjige in Knjige)
			{
				if (naslov == imeKnjige.naziv)
				{
					Console.WriteLine("Knjiga već postoji! Molim ponovno unesite naziv knjige!");
					Console.Clear();
					System.Threading.Thread.Sleep(500);
					Console.Clear();
					goto ponovno1;
				}
			}
			again:
			Console.WriteLine("\nOdaberite opciju 1 ili 2.");
			Console.WriteLine("\n1-->Odaberite postojeceg autora: ");
			Console.WriteLine("\n2-->Dodajte novog autora");
			odabir1 = Convert.ToInt32(Console.ReadLine());
			switch (odabir1)
			{
				case 1:
					{
						Console.WriteLine("\nPostojeci autori:");
						IspisiAutore();
						Console.WriteLine("Izaberite broj autora koji želite odabrati...");
						Console.WriteLine("Unesite broj autora koliko ih je napisalo knjigu");
						int brojAutora = Convert.ToInt32(Console.ReadLine());
						for (int i = 1; i <= brojAutora; i++)
						{
							odabirAutora = Console.ReadLine();
						}
						foreach (var Autor in Autori)
						{
							if (Autor.redniBroj == odabirAutora)
							{
								AutorB = Autor.imeAutora;
								rBroj = Autor.redniBroj;
							}
						}
						break;
					}
				case 2:
					{

						Console.WriteLine("\nDodavanje novog autora");
						Console.WriteLine("Unesite broj autora koliko ih je napisalo knjigu");
						int brojAutora = Convert.ToInt32(Console.ReadLine());
						for (int i = 1; i <= brojAutora; i++)
						{
							Console.WriteLine("Napiši prezime, ime {0}. autora...", i);
							AutorB = Console.ReadLine();
							int brojID = BrojanjeAutora();
							brojID++;
							rBroj= Convert.ToString(brojID);
						
						}
						foreach (var autorime in Autori)
						{
							if (AutorB == autorime.imeAutora)
							{
								Console.WriteLine("Autor već postoji! Sada će se pojaviti izbornik u kojem možete dodati postojećeg autora.");
								System.Threading.Thread.Sleep(500);
								goto again;
							}
						}
						break;
					}
				default:
					{
						goto again;
						break;
					}
			}
			Console.WriteLine("Unesite šifru knjige: ");
			sifra = Console.ReadLine();
			ponovno:
			Console.WriteLine("Unesite godinu izdavanja knjige: ");
			godIzdavanja = Console.ReadLine();
			int godina = Convert.ToInt32(godIzdavanja);
			int trenutnaGod = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
			if (godina > trenutnaGod)
			{
				Console.WriteLine("Nevaljana godina!! Molim pokušajte ponovno!");
				goto ponovno;
			}
			if (odabir1 == 2)
			{
				UpisiAutora(AutorB, rBroj);
			}
			UpisiKnjige(sifra, naslov, rBroj, godIzdavanja);

			Console.Clear();
			TkojePrijavljen();

		}
		public static int BrojanjeAutora()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja broji autore u listi Autori
			 * 
			 * ----------------------------------------------
			 * 
			 */
			int brojID = 0;
			foreach(var BrojAutor in Autori)
			{
				brojID++;
			}
			return brojID;
		}
		public static void IspisiAutore()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja ispisuje sve autore iz liste 
			 * Autori.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			DohvatiAutore();
			int redniBroj = 1;
			foreach (var Autor in Autori)
			{
				Console.WriteLine(redniBroj++ + ". " + Autor.imeAutora);
			}
		}
		public static void UpisiAutora(string imeprezime, string redniBroj)
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja omogućuje upis novog autora u 
			 * XML datoteku.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			var doc = XDocument.Load("autori.xml");
			var noviElement = new XElement("autori",
							new XAttribute("redni_broj", redniBroj),
							new XAttribute("autor", imeprezime)
				);
			doc.Element("data").Add(noviElement);
			doc.Save("autori.xml");
		}
		public static void UpisiKnjige(string sifra, string naziv, string redniBrojID, string godinaIzdavanja)
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja omogućuje upis nove knjige u 
			 * XML datoteku.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			var doc = XDocument.Load("knjige.xml");
			var noviElement = new XElement("knjige",
							new XAttribute("sifra", sifra),
							new XAttribute("naziv", naziv),
							new XAttribute("autorID", redniBrojID),
							new XAttribute("godina_izdavanja", godinaIzdavanja)
				);
			doc.Element("data").Add(noviElement);
			doc.Save("knjige.xml");
		}
		public static void ObrisiKnjige()
		{	/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja omogućuje korisniku brisanje 
			 * brisanje određene knjige.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			string tekst = "Pokrenuto je brisanje knjiga";
			Loganje(tekst);
			Console.WriteLine("Unesite šifru knjige koju želite obrisati...");
			Console.Write("Šifra knjige:");
			long d = Convert.ToInt64(Console.ReadLine());
			string sXml = "";
			StreamReader oSr = new StreamReader("knjige.xml");
			using (oSr)
			{
				sXml = oSr.ReadToEnd();
			}
			XmlDocument oXml = new XmlDocument();
			oXml.LoadXml(sXml);
			XmlNodeList oNodes = oXml.SelectNodes("//data/knjige");
			int signal = 0;
			foreach (XmlNode oNode in oNodes)
			{
				long id = Convert.ToInt64(oNode.Attributes["sifra"].Value);
				if (id == d)
				{
					oXml.DocumentElement.RemoveChild(oNode);
					if (id == d)
					{
						Console.WriteLine("Vaša knjiga {0} uspješno je obrisana!", oNode.Attributes["sifra"].Value);
						signal = 1;
					}
				}
			}
			if (signal == 0)
			{
				Console.WriteLine("Knjiga ne postoji!");
			}
			oXml.Save("knjige.xml");
			Console.Clear();
			TkojePrijavljen();
		}
		public static void PretrazivanjeKnjiga()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja omogućuje korisniku pretraživanje
			 * određene knjige.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			int redniBroj = 1;
			string tekst = "Pokrenuto je pretraživanje knjiga";
			Loganje(tekst);
			DohvatiAutore();
			DohvatiKnjige();
			var ttable = new ConsoleTable("Rbr", "Šifra", "Naziv", "Autor", "Godina Izdavanja");
			Console.WriteLine("1-po dijelu naziva\n2-po dijelu naziva autora\n3-po godini(manje - više)\nENTER-Izlaz na glavni izbornik");
			string izbor;
			int odabir1;
			izbor = Console.ReadLine();
			if (izbor == "")
			{
				Console.Clear();
				TkojePrijavljen();
			}
			odabir1 = Convert.ToInt32(izbor);
			switch (odabir1)
			{
				case 1:
					{
						Console.Clear();
						izbor1:
						Console.Write("Unesite dio naziva knjige: ");
						string nazivDio = Console.ReadLine();
						if (nazivDio == "")
						{
							Console.Clear();
							goto izbor1;
						}
						foreach (var Knjiga in Knjige)
						{
							if (Knjiga.naziv.Contains(nazivDio.ToUpper()))
							{
								foreach (var Autor in Autori)
								{
									if (Knjiga.autorID == Autor.redniBroj)
									{
										ttable.AddRow(redniBroj++ ,Knjiga.sifra, Knjiga.naziv, Autor.imeAutora, Knjiga.godinaIzadavanja);
									}
								}
							}
						}
						ttable.Write();
						Console.ReadKey();
						break;
					}
				case 2:
					{
						Console.Clear();
						izbor2:
						Console.Write("Unesite dio naziva autora: ");
						string nazAutor = Console.ReadLine();
						if (nazAutor == "")
						{
							Console.Clear();
							goto izbor2;
						}
						foreach (var Autor in Autori)
						{
							if (Autor.imeAutora.ToLower().Contains(nazAutor.ToLower()))
							{
								foreach (var Knjiga in Knjige)
								{
									if (Knjiga.autorID == Autor.redniBroj)
									{
										ttable.AddRow(redniBroj++ ,Knjiga.sifra, Knjiga.naziv, Autor.imeAutora, Knjiga.godinaIzadavanja);
									}
								}
							}
						}
						ttable.Write();
						Console.ReadKey();
						break;
					}
				case 3:
					{
						Console.Clear();
						izbor3:
						Console.Write("Unesite godinu izdanja: ");
						string godinaIz = Console.ReadLine();
						int ukupnoKnjiga = 0;
						int BrGod = 0;
						if (godinaIz == "")
						{
							Console.Clear();
							goto izbor3;
						}
						foreach (var Knjiga in Knjige)
						{
							
							if (Knjiga.godinaIzadavanja == godinaIz)
							{
								foreach (var Autor in Autori)
								{
									if (Knjiga.autorID == Autor.redniBroj)
									{
										ttable.AddRow(redniBroj++ ,Knjiga.sifra, Knjiga.naziv, Autor.imeAutora, Knjiga.godinaIzadavanja);
										BrGod++;
									}
								}
							}
							ukupnoKnjiga++;
						}
						ttable.Write();
						double postotak = (Convert.ToDouble(BrGod) / Convert.ToDouble(ukupnoKnjiga)*100);
						Console.WriteLine("Postotak knjiga {0}. godine od sveukupnog broja knjiga je: ", godinaIz);
						Console.WriteLine("{0}%", postotak);
						//Console.Write(postotak.ToString("P2", CultureInfo.InvariantCulture));
						Console.ReadKey();
						break;
					}
				default:
					{
						break;
					}
			}
		}
		public static void SortiranjePoAutoru()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja ispisuje sve knjige u sustavu,
			 * ali su sortirane po autorima.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			string tekst = "Sortirane su knjige po autoru";
			Loganje(tekst);
			DohvatiAutore();
			DohvatiKnjige();
			var ttable = new ConsoleTable("Rbr", "Šifra", "Naziv", "Autor", "Godina Izdavanja");
			int redniBroj = 1;
			List<KnjigaA> KnjigesAutorom = new List<KnjigaA>();
			KnjigesAutorom.Clear();
			foreach (var Knjiga in Knjige)
			{
				foreach (var Autor in Autori)
				{
					if (Knjiga.autorID == Autor.redniBroj)
					{
						KnjigesAutorom.Add(new KnjigaA(Knjiga.sifra, Knjiga.naziv, Autor.imeAutora, Knjiga.godinaIzadavanja));
					}
				}
			}
			KnjigesAutorom = KnjigesAutorom.OrderBy(x => x.autor).ToList();
			foreach (var Knjiga in KnjigesAutorom)
			{
				ttable.AddRow(redniBroj++, Knjiga.sifra, Knjiga.naziv, Knjiga.autor, Knjiga.godinaIzadavanja);
			}
			ttable.Write();
			Console.ReadKey();
			Console.Clear();
		}
		public static void DohvatiKnjige()
		{	/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja sprema knjige iz XML datoteke 
			 * knjige.xml u listu Knjige
			 * 
			 * ----------------------------------------------
			 * 
			 */
			Knjige.Clear();
			string sXml = "";
			StreamReader oSr = new StreamReader(@"knjige.xml");
			using (oSr)
			{
				sXml = oSr.ReadToEnd();
			}
			XmlDocument oXml = new XmlDocument();
			oXml.LoadXml(sXml);
			XmlNodeList oNodes = oXml.SelectNodes("//data/knjige");
			foreach (XmlNode oNode in oNodes)
			{
				brojKnjiga += 1;
				Knjige.Add(new Knjiga(oNode.Attributes["sifra"].Value, oNode.Attributes["naziv"].Value, oNode.Attributes["autorID"].Value, oNode.Attributes["godina_izdavanja"].Value));
			}
		}
		public static void DohvatiAutore()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja sprema autore iz XML datoteke 
			 * autori.xml u listu Autori.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			Autori.Clear();
			int redniBroj = 1;
			string sXml = "";
			StreamReader oSr = new StreamReader(@"autori.xml");
			using (oSr)
			{
				sXml = oSr.ReadToEnd();
			}
			XmlDocument oXml = new XmlDocument();
			oXml.LoadXml(sXml);
			XmlNodeList oNodes = oXml.SelectNodes("//data/autori");
			foreach (XmlNode oNode in oNodes)
			{
				Autori.Add(new Autor(Convert.ToString(redniBroj), oNode.Attributes["autor"].Value));
				redniBroj++;
			}
		}
		public static void IspisKnjiga()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja ispisuje sve knjige u sustavu.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			int redniBroj = 1;
			string tekst = "Ispisane su sve knjige";
			Loganje(tekst);
			var ttable = new ConsoleTable("Rbr", "Šifra", "Naziv", "Autor", "Godina Izdavanja");
			foreach (var Knjiga in Knjige)
			{
				foreach (var Autor in Autori) 
				{
					if (Knjiga.autorID == Autor.redniBroj) 
					{
						ttable.AddRow(redniBroj++ , Knjiga.sifra, Knjiga.naziv, Autor.imeAutora, Knjiga.godinaIzadavanja);
					}
				}	
			}
			ttable.Write();
			Console.ReadKey();
			Console.Clear();
			TkojePrijavljen();
		}
		public static void Login() 
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja omogućuje prijavu korisnika.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			string userEnteredUsername, userEnteredPassword;
			string sXml = "";
			StreamReader oSr = new StreamReader(@"config.xml");
			using (oSr)
			{
				sXml = oSr.ReadToEnd();
			}
			XmlDocument oXml = new XmlDocument();
			oXml.LoadXml(sXml);
			XmlNodeList oNodes = oXml.SelectNodes("//data/signin");
			List<Prijava> podaciPrijave = new List<Prijava>();
			foreach (XmlNode oNode in oNodes)
			{
				podaciPrijave.Add(new Prijava(oNode.Attributes["username"].Value, oNode.Attributes["password"].Value));
			}
			//3 puta se moze krivo prijaviti
			for (int i = 0; i < 3; i++)
			{
				Console.Write("Unesi korisničko ime:");
				userEnteredUsername = Console.ReadLine();
				Console.Write("Unesi lozinku:");
				userEnteredPassword = Console.ReadLine();
				foreach (var podatakZaPrijava in podaciPrijave)
				{
					if (userEnteredPassword == podatakZaPrijava.lozinka && userEnteredUsername == podatakZaPrijava.korisnickoIme)
					{
						Console.WriteLine("Uspjesno ste se prijavili!");
						i = 3;
						prijavljen = true;
						System.Threading.Thread.Sleep(750);
						if (userEnteredUsername == "admin")
						{
							adminPrij = true;
							signal = 1;
						}
						Console.Clear();
					}
				}
				if (prijavljen == false)
				{
					Console.WriteLine("Unjeli ste krivu lozunku ili korisnicko ime");
					System.Threading.Thread.Sleep(750);
					Console.Clear();
				}
			}
			if (prijavljen == false)
			{
				Console.WriteLine("Unjeli ste krivu lozinku ili korisnicko ime 3 puta za redom program se isključuje");
				System.Threading.Thread.Sleep(1000);
				Environment.Exit(0);
			}
		}
		public static void TkojePrijavljen()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja provjerava koji je tip korisnika
			 * prijavljen u sustav.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			if (signal == 1)
			{
				AdminIzbornik();
			}
			else
			{
				KorisnikIzbornik();
			}
		}
		public static void AdminIzbornik()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja prikazuje izbornik
			 * administratora.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			string tekst = "Prijava u sustav (Administrator)";
			Loganje(tekst);
			Console.WriteLine("Odaberite akciju: \n 1 - Prikaži sve knjige \n 2 - Pretraži knjige \n 3 - Sortiraj knjige \n 4 - Dodaj knjigu \n 5 - Obriši knjigu \n 6 - Statistika \n 7 - Odjava");
			string izbor;
			int odabir;
			izbor = Console.ReadLine();
			if (izbor == "")
			{
				Console.Clear();
				TkojePrijavljen();
			}
			odabir = Convert.ToInt32(izbor);
			switch (odabir)
			{
				case 1:
					Console.Clear();
					DohvatiAutore();
					DohvatiKnjige();
					IspisKnjiga();
					break;
				case 2:
					Console.Clear();
					PretrazivanjeKnjiga();
					break;
				case 3:
					Console.Clear();
					SortiranjePoAutoru();
					break;
				case 4:
					Console.Clear();
					Console.Write("Dodavanje knjige");
					DodavanjeKnjige();
					break;
				case 5:
					Console.Clear();
					Console.Write("Brisanje knjige");
					ObrisiKnjige();
					break;
				case 6:
					Console.Clear();
					Console.Write("Statistika");
					Statistika();
					break;
				case 7:
					string tekst1 = "Odjava iz sustava";
					Loganje(tekst1);
					Console.Clear();
					Console.Write("Odjava");
					for (int i = 0; i < 5; i++)
					{
						Console.Write(".");
						System.Threading.Thread.Sleep(500);
					}
					System.Threading.Thread.Sleep(500);
					Environment.Exit(0);
					break;
				default:
					Console.Clear();
					TkojePrijavljen();
					break;
			}
			Console.Clear();
			TkojePrijavljen();
		}
		public static void KorisnikIzbornik()
		{
			/*
			 * 
			 * ----------------------------------------------
			 * 
			 * Funkcija koja prikazuje izbornik
			 * korisnika.
			 * 
			 * ----------------------------------------------
			 * 
			 */
			string tekst = "Prijava u sustav (Korisnik)";
			Loganje(tekst);
			string izbor;
			int odabir;
			Console.WriteLine("Odaberite akciju: \n 1 - Prikaži sve knjige \n 2 - Pretraži knjige \n 3 - Sortiraj knjige \n 4 - Statistika \n 5 - Odjava");
			izbor = Console.ReadLine();
			if (izbor == "")
			{
				Console.Clear();
				TkojePrijavljen();
			}
			odabir = Convert.ToInt32(izbor);
			switch (odabir)
			{
				case 1:
					Console.Clear();
					DohvatiAutore();
					DohvatiKnjige();
					IspisKnjiga();
					break;
				case 2:
					Console.Clear();
					PretrazivanjeKnjiga();
					break;
				case 3:
					Console.Clear();
					SortiranjePoAutoru();
					break;
				case 4:
					Console.Clear();
					Statistika();
					break;
				case 5:
					string tekst1 = "Odjava iz sustava";
					Loganje(tekst1);
					Console.Clear();
					Console.Write("Odjava");
					for (int i = 0; i < 5; i++)
					{
						Console.Write(".");
						System.Threading.Thread.Sleep(500);
					}
					System.Threading.Thread.Sleep(500);
					Environment.Exit(0);
					break;
				default:
					Console.Clear();
					TkojePrijavljen();
					break;
			}
			Console.Clear();
			TkojePrijavljen();
		}
		public static void Main(string[] args)
		{
			string tekst = "Program pokrenut";
			Loganje(tekst);
			Login();
			if (prijavljen == true) 
			{
				if (adminPrij == true)
				{
					AdminIzbornik();
				}
				else
				{
					KorisnikIzbornik();
				}	
			}
				Console.WriteLine("Press any key to continue . . . ");
				Console.ReadKey();
			}
		}
	}
	 
	 
	 
	 