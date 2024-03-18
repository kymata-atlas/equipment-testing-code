% generate 3D rgb-XYZ mapping matrix

% 15x15x15 is 3375

count=1;
colour_table = [];
for r = 0:17:255
    for g = 0:17:255
        for b = 0:17:255
            colour_table(count, 1) = r;
            colour_table(count, 2) = g;
            colour_table(count, 3) = b;
            count = count+1;
        end
    end
end
scatter3(colour_table(:,1),colour_table(:,2),colour_table(:,3));
writematrix(colour_table,'test_file_rgb_color_array.csv') 


% ----- Once the table has been obtained with the colometer
scatter3(rgbxyzcolorarray{:,'CIE1931X'},rgbxyzcolorarray{:,'CIE1931Y'},rgbxyzcolorarray{:,'CIE1931Z'});