Public Class ExportDocToPDF

    Private _Error As String

    Public Function GetError() As String
        Return _Error
    End Function
    Public Function DoExport(strSourceFullPath As String, strDestFullPath As String, strServiceProfile As String) As Boolean
        'strServiceProfile by mělo být: profile002.ini
        Dim c As New Print2PdfWebService2SoapClient()
        Dim cFile As New clsFile()


        Dim bytesInput As Byte() = cFile.ReadByteArrayFromFile(strSourceFullPath)
        Dim str64 As String = Convert.ToBase64String(bytesInput)    'nutný převod obsahu do base64!!!

        bytesInput = Convert.FromBase64String(str64)

        Dim bytesOutput As Byte() = Nothing
        Dim strReport As String = ""
        Try
            Dim x As Integer = c.ConvertFile(bytesInput, cFile.GetNameFromFullpath(strSourceFullPath), strServiceProfile, bytesOutput, strReport)
            If x > 0 Or bytesOutput Is Nothing Then
                'chyba
                _Error = "Chyba při generování PDF výstupu, x=" & x.ToString
                Return False
            End If
        Catch ex As Exception
            _Error = ex.Message
            Return False
        End Try


        Try
            cFile.WriteFileFromArray(strDestFullPath, bytesOutput)

        Catch ex As Exception
            _Error = ex.Message
            Return False
        End Try

        Return True

    End Function

End Class
