import 'package:postgres/postgres.dart';

class Database {
  static final Database _instance = Database._internal();
  late Connection conn;
  factory Database() {
    return _instance;
  }

  Database._internal();

  Future<void> connect() async {
    conn = await Connection.open(
      Endpoint(
        host: 'isabelle.db.elephantsql.com',
        database: 'zrmfzafr',
        username: 'zrmfzafr',
        password: '2F0bg6YYX51w_NRMFvO8Lhw3PrMBpW0s',
      ),
      settings: const ConnectionSettings(sslMode: SslMode.verifyFull),
    );
  }

  Future<void> close() async {
    await conn.close();
  }

  Future<Result> checkLogin(user, psw) async {
    try{
      await connect();
      
      final result = await conn.execute(
        Sql.named('SELECT UserID, UserName, UserEmail, UserLevel, UserPoints FROM tbUsers WHERE LOWER(UserName) = LOWER(@user) AND UserPassword = MD5(@psw)'),
        parameters: {'user' : user, 'psw' : psw}, 
      );

      await close();
      return result;

    } catch (e) {
      await close();
      throw 'Error while trying to connect with database: $e';
    }
  }

  Future<Result> insertUser(user, psw, email) async {
    try {
      await connect();
      
     final checkUser = await conn.execute(
      Sql.named('SELECT UserName FROM tbUsers WHERE LOWER(UserName) = LOWER(@user)'),
      parameters: {'user' : user}
     );

     if (checkUser.affectedRows == 0){
        final result = await conn.execute(
          Sql.named('INSERT INTO tbUsers (UserName, UserPassword, UserEmail, UserLevel, UserPoints) VALUES (@user, md5(@psw), @email, 0, 0);'),
          parameters: {
            'user'  : user, 
            'psw'   : psw,
            'email' : email,
          }, 
        );
        await close();
        return result;
      } else {
        return Result(affectedRows: -1, rows: [], schema: ResultSchema([]));
      }
    } catch (e) {
      await close();
      throw 'Error while trying to connect with database: $e';
    }
  }

  Future<Result> addPoints(userID, points) async {
    try {
      await connect();
      
      final insertResult = await conn.execute(
        Sql.named('UPDATE tbUsers SET UserPoints = (UserPoints::INTEGER + @points)::VARCHAR WHERE UserID = @userID'),
        parameters: {
          'userID' : userID,
          'points' : points
        }
      );

      if(insertResult.affectedRows >= 0) {
        final result = await conn.execute(
          Sql.named('SELECT UserID, UserName, UserEmail, UserLevel, UserPoints FROM tbUsers WHERE UserID = @userID'),
          parameters: {'userID' : userID}
        );

        String level = (int.parse(result[0][4].toString()) / 1000).toString().split('.')[0];
        await conn.execute(
          Sql.named('UPDATE tbUsers SET UserLevel = @level WHERE UserID = @userID'),
          parameters: {
            'userID' : userID,
            'level' : level
          }
        );

        await close();
        return result;
      }
      return Result(rows: [], affectedRows: -1, schema: ResultSchema([]));
    } catch (e) {
      await close();
      throw 'Error while adding points: $e';
    }
  }

  Future<Result> changePsw(userID, oldPsw, newPsw) async {
    try {
      await connect();
      
      final selectResult = await conn.execute(
        Sql.named('SELECT UserPassword FROM tbUsers WHERE UserID = @userID and UserPassword = MD5(@oldPsw)'),
        parameters: {
          'userID' : userID,
          'oldPsw' : oldPsw
        }
      );

      if(selectResult.affectedRows > 0) {
        final result = await conn.execute(
          Sql.named('UPDATE tbUsers SET UserPassword = MD5(@newPsw) WHERE UserID = @userID'),
          parameters: {
            'userID' : userID,
            'newPsw' : newPsw
          }
        );
        
        if (result.affectedRows >= 0) {
          await close();
          return result;
        }
      } else {
        await close();
        return Result(rows: [], affectedRows: -1, schema: ResultSchema([]));
      }
      await close();
      return Result(rows: [], affectedRows: -2, schema: ResultSchema([]));
    } catch (e) {
      await close();
      throw 'Error on update: $e';
    }
  }

  Future<Result> updateUser(userID, username, email, skipCheck) async {
    try {
      await connect();

      final checkUser = await conn.execute(
      Sql.named('SELECT UserName FROM tbUsers WHERE LOWER(UserName) = LOWER(@user)'),
      parameters: {'user' : username}
     );

      if (skipCheck ? true : checkUser.affectedRows == 0) {
        final result = await conn.execute(
          Sql.named('UPDATE tbUsers SET UserName = @username, UserEmail = @email WHERE UserID = @userID'),
          parameters: {
            'userID' : userID,
            'username' : username,
            'email' : email 
          }
        );
        if (result.affectedRows >= 0) {
          final select = await conn.execute(
            Sql.named('SELECT UserID, UserName, UserEmail, UserLevel, UserPoints FROM tbUsers WHERE UserID = @userID'),
            parameters: {'userID' : userID}
          );
         
          if (select.affectedRows > 0) {
            await close();
            return select;
          }
        }
      } else {
        await close();
        return Result(rows: [], affectedRows: -1, schema: ResultSchema([]));
      }

      await close();
      return Result(rows: [], affectedRows: -2, schema: ResultSchema([]));
    } catch (e) {
      await close();
      throw 'Error on update: $e';
    }
  }

  Future<Result> getLanguages() async {
    try{
      await connect();
      
      final result = await conn.execute(
        Sql('SELECT LanguageName FROM tbLanguages'),
      );

      await close();
      return result;
    } catch (e) {
      await close();
      throw 'Error while trying to connect with database: $e';
    }
  }
}