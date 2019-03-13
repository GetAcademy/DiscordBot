# DiscordBot
GETsharp Discord bot readme. Dette er en bot for startIT/getIT kurs for å hjelpe lærere og elever i kursets løp.
***
#### Fremdriftsplan
- [ ] Crash/Shutdown handler program
- [ ] Mer info som sendes til elever
- [ ] Interaktivitet gjennom DM med elever
- [ ] Automatiske påminnelser om video osv til elever som mangler
- [ ] "Oppmøte" registrering
- [ ] Automatisk anngi roller til nye elever
- [ ] Generering av invite linker
- [ ] Sende invite linker i DM
- [ ] Fjerne invokerende meldinger
- [ ] Sette "synlighet" på admin lvl kommandoer så de bare synes for admins
- [ ] Sende Template filer til brukere i DM
- [ ] Sende ressursjlinker til brukere i DM
- [ ] Registrere oppmøte via kommando
- [ ] Registrere video via kommando/evt via DM fra elev til BOT-account
***
####  Milestones
* <s>Basic Logging og kontakt med Discord server</s>
* <s>Basic Interaktivitet</s>
* <s>Basic Interakivitet gjennom DM</s>
* Avansert Interaktivitet i chat gjennom kommandoer med flere parametere
* Automatiske påminnelser i general Chat
* Automatiske påminnelser i DM (tidsbasert)
* Automatiske påminnelser i DM basert på oppmøte/Videoregistrering
*** 
#### Additional
- Mulighet til å "videresende" DM meldinger fra elever til en, eller flere bot master kontoer. 
- Mulighet for at "Bot masters" (lagre id lokalt på bot) kan sende meldinger/kommandoer direkte til bot for f.eks. sende påminnelser, info
- Lage en "Kanal" (DM) Hvor elever kan sende spørsmål til botten, disse videresendes til Marius & Terje som kan svare og sende tilbake en respons
- Når en elev blir med på serveren, vil eleven få beskjed om å svare på meldingen med navnet sitt. Dermed kan botten registrere brukerens discord ID for å binde det opp mot data i elevoversikt. Dette vil forenkle oppmøte og videoregistrering.
- Registrering av brukere kan skje ved at en admin starter en kommando, f.eks. @GET RegisterCurrentUsers. Denne vil da først få en oversikt over alle brukere på serveren, for så gå igjennom dem en av gangen og sjekke om \<SocketGuildUser\> user.VoiceChannel ikke er "null" og er en av serverens registrerte kanaler. 
- Lagt til mulighet for at studenter kan sende spørsmål til Bot som registreres og lagres.
- Lagt til broadcast melding. DM til elever fra en lærer
***
#### How to
***
