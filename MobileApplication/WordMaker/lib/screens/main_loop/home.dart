import 'dart:io';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:hive_flutter/hive_flutter.dart';
import 'package:path_provider/path_provider.dart';
import 'package:word_maker/screens/main_loop/discovery_game.dart';
import 'package:word_maker/screens/main_loop/game_tutorial.dart';
import 'package:word_maker/screens/profile_control/profile.dart';
import 'package:word_maker/screens/session_control/login.dart';

class HomeScreen extends StatefulWidget {
  final Map<String, dynamic> userData;
  final Box dataBox;
  const HomeScreen({super.key, required this.dataBox, required this.userData});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  File? imgFile;
  Map<String, dynamic> userData = {};
  late Box dataBox;

  void getImage() async {
    final Directory appDir = await getApplicationDocumentsDirectory();
    final String appDirPath = appDir.path;
    final File imageFile = File('$appDirPath/profile_image.png');

    if (await imageFile.exists()) {
      setState(() {
        imgFile = imageFile;
      });
    }
  }

  @override
  void initState() {
    super.initState();
    setState(() {
      userData = widget.userData;
    });
    dataBox = widget.dataBox;
    getImage();
  }

  @override
  Widget build(BuildContext context) {
    void showBackDialog() {
      showDialog(
        context: context,
        builder: (BuildContext context) {
          var height = MediaQuery.sizeOf(context).height;

          return AlertDialog(
            title: Text(
              'Leaving the app',
              style: TextStyle(
                fontSize: height * 0.03,
                color: Colors.black,
              ),
            ),
            content: Text(
              'Are you sure you want to leave this app?',
              style: TextStyle(
                fontSize: height * 0.025
              ),
            ),
            actions: [
              Row(
                children: [
                  TextButton(
                    child: Text(
                      'Leave',
                      style: TextStyle(
                        fontSize: height * 0.025,
                        color: Colors.black
                      ),
                    ),
                    onPressed: () {
                      SystemNavigator.pop();
                    },
                  ), 
                  SizedBox(width: height * 0.12,),
                  TextButton(
                    child: Text(
                      'Cancel',
                      style: TextStyle(
                        color: Colors.black,
                        fontSize: height * 0.025,
                      ),
                    ),
                    onPressed: () {
                      Navigator.pop(context);
                    },
                  ), 
                ],
              ),
            ],
          );
        },
      );
    }
        
    return LayoutBuilder(
      builder: (context, constraints) {
        double deviceWidth = constraints.maxWidth;
        double deviceHeight = constraints.maxHeight;

        return DefaultTabController(
          initialIndex: 0,
          length: 1,
          child: Scaffold(
            drawerEnableOpenDragGesture: true,
            appBar: AppBar(
              backgroundColor: Colors.blue[800],
              foregroundColor: Colors.white,
              toolbarHeight: deviceHeight * 0.095,
              title: Text(
                'Word Maker', 
                style: GoogleFonts.caveat(
                  textStyle: TextStyle(
                    fontSize: deviceHeight * 0.04,
                  ) 
                )
              ),
              notificationPredicate: (ScrollNotification notification) {
                return notification.depth == 1;
              },
              scrolledUnderElevation: 4,
              shadowColor: Colors.white,
              bottom: TabBar(
                labelColor: Colors.tealAccent,
                indicatorColor: Colors.tealAccent,
                unselectedLabelColor: Colors.white,
                indicatorSize: TabBarIndicatorSize.tab,
                indicatorWeight: 5,
                labelStyle: TextStyle(
                  fontSize: deviceHeight * 0.025,
                ),
                tabs: [
                  Tab(
                    icon: Icon(Icons.construction_outlined, size: deviceHeight * 0.045,),
                    text: 'Word Maker',
                  ),
                ]
              ),
              actions: [
                IconButton(
                    tooltip: 'How do I play?',
                    onPressed: (){
                      Navigator.push(context, 
                      MaterialPageRoute(builder: (context) => const TutorialScreen(),));
                    }, 
                    icon: Icon(
                      Icons.help_outline,
                      size: deviceHeight * 0.035,
                    ),
                  ),
              ],
            ),
            onDrawerChanged: (isOpened) {
              if (isOpened) {
                getImage();
              }
            },
            drawer: Drawer(
              width: deviceWidth * 0.7,
              surfaceTintColor: Colors.lightBlue,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  Container(
                    color: Colors.blue,
                    padding: EdgeInsets.only(top: deviceHeight * 0.075),
                    child:  Column(
                      children: [    
                        Hero(
                          tag: 'profileImage', 
                          child: CircleAvatar(
                            radius: deviceHeight * 0.08,
                            backgroundImage: imgFile != null && imgFile!.existsSync() ? Image.file(imgFile!).image : const AssetImage('assets/images/no_image.png'),
                          ),
                        ),                    
                        SizedBox(height: deviceHeight * 0.01),
                        Text(
                          userData['User'].toString().substring(0,1).toUpperCase() + userData['User'].toString().substring(1),
                          style: TextStyle(
                            fontSize: deviceHeight * 0.035,
                            color: Colors.white,
                          ),
                        ),
                        Text(
                          userData['Email'],
                          style: TextStyle(
                            fontSize: deviceHeight * 0.02,
                            color: Colors.white,
                          ),
                        ),
                        SizedBox(height: deviceHeight * 0.01,),
                        Divider(thickness: 2,endIndent: deviceHeight * 0.03,indent: deviceHeight * 0.03,),
                        SizedBox(height: deviceHeight * 0.01,),
                        Text('Level: ${userData['Level']}',
                          style: TextStyle(
                            fontSize: deviceHeight * 0.03,
                            color: Colors.white,
                          ),
                        ),
                        Text('Points: ${userData['Points']}',
                          style: TextStyle(
                            fontSize: deviceHeight * 0.03,
                            color: Colors.white,
                          ),
                        ),
                        SizedBox(height: deviceHeight * 0.01,)
                      ],
                    ),
                  ),
                  Container(
                    padding: EdgeInsets.symmetric(vertical: deviceHeight * 0.05, horizontal: deviceHeight * 0.02),
                    child: Wrap(
                      runSpacing: deviceHeight * 0.016,
                      children: [
                        ListTile(
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(30)),
                          leading: Icon(Icons.person_outline, size: deviceHeight * 0.04,),
                          title: Text(
                            'Profile',
                            style: TextStyle(
                              fontSize: deviceHeight * 0.028,
                            ),
                          ),
                          onTap: (){
                            Navigator.pop(context);
                            Navigator.of(context).push( 
                              MaterialPageRoute(
                                builder: (context) => ProfileScreen(dataBox: widget.dataBox, userData: widget.userData,),
                              )
                            );
                          },
                        ),
                        SizedBox(height: deviceHeight * 0.35,),
                        ListTile(
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(30)),
                          leading: Icon(Icons.logout_rounded, size: deviceHeight * 0.038,),
                          title: Text(
                            'Log out',
                            style: TextStyle(
                              color: Colors.red,
                              fontSize: deviceHeight * 0.028,
                            ),
                          ),
                          onTap: (){
                            showDialog(
                              context: context, 
                              builder: (context) {
                                return AlertDialog(
                                  title: Text(
                                    'Leaving the app',
                                    style: TextStyle(
                                      fontSize: deviceHeight * 0.03,
                                      color: Colors.black,
                                    ),
                                  ),
                                  content: Text(
                                    'Are you sure you want to log out?',
                                    style: TextStyle(
                                      fontSize: deviceHeight * 0.025
                                    ),
                                  ),
                                  actions: [
                                    Row(
                                      children: [
                                        TextButton(
                                          child: Text(
                                            'Yes',
                                            style: TextStyle(
                                              fontSize: deviceHeight * 0.025,
                                              color: Colors.black
                                            ),
                                          ),
                                          onPressed: () {
                                            Navigator.pushAndRemoveUntil(context, 
                                              MaterialPageRoute(builder: (context) => const LoginScreen(),), 
                                              ModalRoute.withName('/')
                                            );
                                          },
                                        ), 
                                        SizedBox(width: deviceHeight * 0.165,),
                                        TextButton(
                                          child: Text(
                                            'No',
                                            style: TextStyle(
                                              color: Colors.black,
                                              fontSize: deviceHeight * 0.025,
                                            ),
                                          ),
                                          onPressed: () {
                                            Navigator.pop(context);
                                          },
                                        ), 
                                      ],
                                    ),
                                  ],
                                );
                              },
                            );
                          },
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
            body: PopScope( 
              canPop: false,
              onPopInvoked: (bool didPop) {
                if (didPop) return;
                showBackDialog();
              },
              child: LayoutBuilder(
                builder: (context, constraints) {
                  double deviceWidth = constraints.maxWidth;
                  double deviceHeight = constraints.maxHeight;
                  
                  return Stack( 
                    children: [
                      Container(
                        decoration: const BoxDecoration(
                          gradient: LinearGradient(
                            colors: [Colors.deepPurple, Colors.blueAccent],
                            begin: Alignment.topRight,
                            end: Alignment.bottomRight
                          ),
                        ),
                        child: TabBarView(
                          children: [
                            ListView.builder(
                              itemCount: 51,
                              padding: EdgeInsets.symmetric(horizontal: deviceWidth * 0.1, vertical: deviceHeight * 0.05),
                              itemBuilder: (context, index) {
                                bool isLastItem = index == 50;
                                Widget subtitleText;
                                String wordLength = widget.dataBox.get('Word${index+1}:Length', defaultValue: '?').toString();
                                String wordCount = widget.dataBox.get('Word${index+1}:Count', defaultValue: '0').toString();


                                if (index < 20) {
                                  subtitleText = Text('Easy (5 Letters)', style: TextStyle(color: Colors.green[800], fontSize: deviceHeight * 0.025, fontWeight: FontWeight.bold));
                                } else if (index < 45) {
                                  subtitleText = Text('Medium (6 Letters)', style: TextStyle(color: Colors.amber[800], fontSize: deviceHeight * 0.025, fontWeight: FontWeight.bold));
                                } else {
                                  subtitleText = Text('Hard (7 Letters)', style: TextStyle(color: Colors.red[800], fontSize: deviceHeight * 0.025, fontWeight: FontWeight.bold));
                                }

                                return Card(
                                  elevation: 4,
                                  shadowColor: Colors.white,
                                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
                                  color: !isLastItem ? Colors.lightBlueAccent.shade100 : Colors.grey.shade400, 
                                  margin: EdgeInsets.only(bottom: deviceHeight * 0.015),

                                  child: Container(
                                    decoration: BoxDecoration(
                                      borderRadius: BorderRadius.circular(20),
                                      gradient: LinearGradient(
                                        colors: !isLastItem ? [Colors.lightBlueAccent.shade100, Colors.lightBlueAccent] : [Colors.grey.shade400, Colors.grey.shade400],
                                        begin: Alignment.centerLeft,
                                        end: Alignment.centerRight
                                      ),
                                    ),
                                      child: ListTile(
                                      key: Key('Word#$index'),
                                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
                                      splashColor: Colors.amber,
                                      onTap: (){
                                        Navigator.pushReplacement(
                                          context,
                                          MaterialPageRoute(builder: (context) => DiscoveryGame(wordLevel: index+1, userData: widget.userData, dataBox: dataBox,),)
                                        );
                                      },
                                      minVerticalPadding: 5,
                                      minTileHeight: deviceHeight * 0.1,
                                      leading: !isLastItem ? Icon(Icons.tag_rounded, size: deviceHeight * 0.06,color: Colors.grey[850],) : null,
                                      title: Center(
                                        child: Text(
                                          textAlign: TextAlign.center,
                                          !isLastItem ? 'Word #${index + 1}' : 'New words coming soon...',
                                          style: TextStyle(
                                            fontWeight: FontWeight.bold,
                                            fontSize: deviceHeight * 0.03,
                                          ),
                                        ),
                                      ),
                                      subtitle: Center(child: !isLastItem ? subtitleText : null),
                                      trailing: !isLastItem ? Text(
                                        '$wordCount/$wordLength',
                                        style: TextStyle(
                                          color: Colors.grey[850],
                                          fontSize: deviceHeight * 0.03,
                                        ),
                                      ) : Icon(Icons.alarm, size: deviceHeight * 0.05,),
                                    ),
                                  )
                                );
                              },
                            ),
                          ],
                        ),
                      ),
                    ],
                  );
                },
              ),
            ),
          ),
        );
      }
    );
  }
}